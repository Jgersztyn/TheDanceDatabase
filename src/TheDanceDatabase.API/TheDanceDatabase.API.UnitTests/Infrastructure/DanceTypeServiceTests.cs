using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using TheDanceDatabase.API.Application;
using TheDanceDatabase.API.Domain.Models;

namespace TheDanceDatabase.API.UnitTests.Infrastructure;

/// <summary>Exercises the real service against the embedded dance-moves.json mock data.</summary>
public class DanceTypeServiceTests : TheDanceDatabaseTest
{
    [Fact]
    public async Task SearchDanceTypes_ShouldReturnStyle_IfStyleDiffersOnlyInCaseAndWhitespace()
    {
        // Arrange
        var request = new DanceTypeSearchRequest { Style = "  hip HOP " };

        // Act
        var result = await SearchDanceTypes(request);

        // Assert
        result.Select(x => x.Name).ShouldBe(["Hip Hop"]);
    }

    [Fact]
    public async Task SearchDanceTypes_ShouldReturnOnlyExactCategory_IfAnotherCategoryContainsTheTerm()
    {
        var request = new DanceTypeSearchRequest { Style = "B-boy/Floor Work", Category = "floor" };

        var result = await SearchDanceTypes(request);

        result.SelectMany(x => x.Categories).Select(x => x.Name).ShouldBe(["Floor"]);
    }

    [Fact]
    public async Task SearchDanceTypes_ShouldPruneEmptyCategoriesAndStyles_IfMoveFilterIsGiven()
    {
        var request = new DanceTypeSearchRequest { Move = "spongebob" };

        var result = await SearchDanceTypes(request);

        Describe(result).ShouldBe("Shuffling: General [Spongebob, Reverse Spongebob]");
    }

    [Fact]
    public async Task SearchDanceTypes_ShouldKeepCategoriesWithoutMoves_IfNoMoveFilterIsGiven()
    {
        var request = new DanceTypeSearchRequest { Style = "Other Styles" };

        var result = await SearchDanceTypes(request);

        Describe(result).ShouldBe("Other Styles: Ballet [Pirouette], Tracing []");
    }

    [Fact]
    public async Task SearchDanceTypes_ShouldReturnEmptyList_IfStyleDoesNotMatch()
    {
        var request = new DanceTypeSearchRequest { Style = "Tap" };

        var result = await SearchDanceTypes(request);

        result.ShouldBeEmpty();
    }

    [Fact]
    public async Task SearchDanceTypes_ShouldReturnEmptyList_IfStyleIsOnlyPartOfAStyleName()
    {
        var request = new DanceTypeSearchRequest { Style = "Work" };

        var result = await SearchDanceTypes(request);

        result.ShouldBeEmpty();
    }

    [Fact]
    public async Task SearchDanceTypes_ShouldReturnOnlyStylesWithTheCategory_IfOnlyCategoryIsGiven()
    {
        var request = new DanceTypeSearchRequest { Category = "general" };

        var result = await SearchDanceTypes(request);

        result.Select(x => x.Name).ShouldBe(["Shuffling", "Arms and Hands (General)"]);
    }

    [Fact]
    public async Task SearchDanceTypes_ShouldReturnEveryDanceType_IfEveryFilterIsBlank()
    {
        var request = new DanceTypeSearchRequest { Style = "", Category = "  ", Move = null };
        var everyDanceType = await GetDanceTypes();

        var result = await SearchDanceTypes(request);

        JsonSerializer.Serialize(result).ShouldBe(JsonSerializer.Serialize(everyDanceType));
    }

    [Fact]
    public async Task GetDanceTypes_ShouldDeserializeMoveNotes_IfMoveHasNotes()
    {
        var result = await GetDanceTypes();

        var move = result.SelectMany(x => x.Categories).SelectMany(x => x.Moves).Single(x => x.Name == "Crossover running man");
        move.Notes.ShouldBe("polly-pocket");
    }

    private async Task<IReadOnlyList<DanceType>> GetDanceTypes()
    {
        await using var scope = Provider.CreateAsyncScope();
        var danceTypeService = scope.ServiceProvider.GetRequiredService<IDanceTypeService>();

        return await danceTypeService.GetDanceTypes(TestContext.Current.CancellationToken);
    }

    private async Task<IReadOnlyList<DanceType>> SearchDanceTypes(DanceTypeSearchRequest request)
    {
        await using var scope = Provider.CreateAsyncScope();
        var danceTypeService = scope.ServiceProvider.GetRequiredService<IDanceTypeService>();

        return await danceTypeService.SearchDanceTypes(request, TestContext.Current.CancellationToken);
    }

    // Flattens the tree to one string so a single assertion covers which styles, categories and moves remain
    private static string Describe(IEnumerable<DanceType> danceTypes)
        => string.Join("; ", danceTypes.Select(danceType => $"{danceType.Name}: " + string.Join(", ",
            danceType.Categories.Select(category => $"{category.Name} [{string.Join(", ", category.Moves.Select(move => move.Name))}]"))));
}
