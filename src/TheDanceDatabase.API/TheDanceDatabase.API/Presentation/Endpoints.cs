namespace TheDanceDatabase.API.Presentation;

public static class Endpoints
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        var danceTypes = app.MapGroup("dance-types")
            .WithTags("Dance Types");

        danceTypes.MapGet("", Functions.GetDanceTypes)
            .WithName(nameof(Functions.GetDanceTypes))
            .WithSummary("Retrieves every supported dance type.");

        return app;
    }
}
