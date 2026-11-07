using System.Text.Json;

namespace TheDanceDatabase.API.UnitTests.Presentation;

/// <summary>Runs the full HTTP pipeline in memory through <see cref="TheDanceDatabaseApplicationFactory"/>.</summary>
public class EndpointsTests : IClassFixture<TheDanceDatabaseApplicationFactory>
{
    private readonly HttpClient _client;

    public EndpointsTests(TheDanceDatabaseApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GettingDanceTypesReturnsEveryStyleFromTheMockData()
    {
        // Act
        var json = await _client.GetStringAsync("dance-types", TestContext.Current.CancellationToken);

        // Assert
        using var document = JsonDocument.Parse(json);
        var names = document.RootElement.EnumerateArray()
            .Select(x => x.GetProperty("name").GetString());

        names.ShouldBe(["Shuffling", "General Footwork", "Waacking", "Hip Hop", "Arms and Hands (General)", "B-boy/Floor Work", "Other Styles"]);
    }

    [Fact]
    public async Task SearchingDanceTypesByStyleAndCategoryReturnsOnlyThatCategory()
    {
        var json = await _client.GetStringAsync("dance-types/search?style=Hip%20Hop&category=Upper%20Body", TestContext.Current.CancellationToken);

        using var document = JsonDocument.Parse(json);
        var styleAndCategoryNames = document.RootElement.EnumerateArray()
            .SelectMany(style => style.GetProperty("categories").EnumerateArray()
                .Select(category => $"{style.GetProperty("name").GetString()} > {category.GetProperty("name").GetString()}"));

        styleAndCategoryNames.ShouldBe(["Hip Hop > Upper Body"]);
    }

    [Fact]
    public async Task GettingDanceTypesFromTheUIOriginIsAllowedByCors()
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, "dance-types");
        request.Headers.Add("Origin", TheDanceDatabaseApplicationFactory.AllowedOrigin);

        using var response = await _client.SendAsync(request, TestContext.Current.CancellationToken);

        response.Headers.GetValues("Access-Control-Allow-Origin").ShouldBe([TheDanceDatabaseApplicationFactory.AllowedOrigin]);
    }
}
