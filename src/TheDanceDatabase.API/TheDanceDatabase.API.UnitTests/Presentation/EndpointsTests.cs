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
    public async Task GettingDanceTypesReturnsSwingJazzAndSalsa()
    {
        // Act
        var json = await _client.GetStringAsync("dance-types", TestContext.Current.CancellationToken);

        // Assert
        using var document = JsonDocument.Parse(json);
        var names = document.RootElement.EnumerateArray()
            .Select(x => x.GetProperty("name").GetString());

        names.ShouldBe(["Swing", "Jazz", "Salsa"]);
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
