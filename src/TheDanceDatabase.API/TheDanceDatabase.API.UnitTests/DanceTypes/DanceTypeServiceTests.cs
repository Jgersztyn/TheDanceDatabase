using Microsoft.Extensions.DependencyInjection;
using TheDanceDatabase.API.Application;

namespace TheDanceDatabase.API.UnitTests.DanceTypes;

public class DanceTypeServiceTests : TheDanceDatabaseTest
{
    [Fact]
    public async Task GetDanceTypes_ShouldReturnSwingJazzAndSalsa_IfServiceIsResolvedFromContainer()
    {
        // Arrange
        await using var scope = Provider.CreateAsyncScope();
        var danceTypeService = scope.ServiceProvider.GetRequiredService<IDanceTypeService>();

        // Act
        var danceTypes = await danceTypeService.GetDanceTypes(TestContext.Current.CancellationToken);

        // Assert
        danceTypes.Select(x => x.Name).ShouldBe(["Swing", "Jazz", "Salsa"]);
    }

    [Fact]
    public async Task GetDanceTypes_ShouldReturnUniqueIds_IfServiceIsResolvedFromContainer()
    {
        await using var scope = Provider.CreateAsyncScope();
        var danceTypeService = scope.ServiceProvider.GetRequiredService<IDanceTypeService>();

        var danceTypes = await danceTypeService.GetDanceTypes(TestContext.Current.CancellationToken);

        danceTypes.Select(x => x.Id).ShouldBeUnique();
    }
}
