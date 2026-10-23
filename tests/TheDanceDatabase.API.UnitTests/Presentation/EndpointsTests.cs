using System.Net;
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
    public async Task GettingDanceTypesSerializesCategoryByName()
    {
        var json = await _client.GetStringAsync("dance-types", TestContext.Current.CancellationToken);

        using var document = JsonDocument.Parse(json);
        var firstCategory = document.RootElement[0].GetProperty("category").GetString();

        firstCategory.ShouldBe("Social");
    }

    [Fact]
    public async Task GettingDanceTypesFromTheUIOriginIsAllowedByCors()
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, "dance-types");
        request.Headers.Add("Origin", TheDanceDatabaseApplicationFactory.AllowedOrigin);

        using var response = await _client.SendAsync(request, TestContext.Current.CancellationToken);

        response.Headers.GetValues("Access-Control-Allow-Origin").ShouldBe([TheDanceDatabaseApplicationFactory.AllowedOrigin]);
    }

    [Fact]
    public async Task GettingDanceTypesFromAnUnknownOriginOmitsCorsHeader()
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, "dance-types");
        request.Headers.Add("Origin", "http://unknown.example");

        using var response = await _client.SendAsync(request, TestContext.Current.CancellationToken);

        response.Headers.Contains("Access-Control-Allow-Origin").ShouldBeFalse();
    }

    [Fact]
    public async Task HealthCheckReportsHealthy()
    {
        var body = await _client.GetStringAsync("health-check", TestContext.Current.CancellationToken);

        body.ShouldBe("Healthy");
    }

    [Fact]
    public async Task RequestingAnUnknownRouteReturnsNotFound()
    {
        using var response = await _client.GetAsync("not-a-route", TestContext.Current.CancellationToken);

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task RequestingAnUnknownRouteReturnsProblemDetails()
    {
        using var response = await _client.GetAsync("not-a-route", TestContext.Current.CancellationToken);

        response.Content.Headers.ContentType.ShouldNotBeNull().MediaType.ShouldBe("application/problem+json");
    }

    [Fact]
    public async Task OpenApiDocumentIsHiddenOutsideDevelopment()
    {
        using var response = await _client.GetAsync("openapi/v1.json", TestContext.Current.CancellationToken);

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}
