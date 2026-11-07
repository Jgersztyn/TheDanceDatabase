using Microsoft.AspNetCore.Http.HttpResults;
using TheDanceDatabase.API.Application;
using TheDanceDatabase.API.Domain.Models;

namespace TheDanceDatabase.API.Presentation;

/// <summary>Handler bodies for the routes mapped in <see cref="Endpoints"/>.</summary>
public static class Functions
{
    /// <summary>Returns every supported dance type.</summary>
    public static async Task<Ok<IReadOnlyList<DanceType>>> GetDanceTypes(IDanceTypeService danceTypeService, CancellationToken cancellationToken)
    {
        var danceTypes = await danceTypeService.GetDanceTypes(cancellationToken);

        return TypedResults.Ok(danceTypes);
    }

    /// <summary>Returns the dance types that match the style, category and move query parameters.</summary>
    public static async Task<Ok<IReadOnlyList<DanceType>>> SearchDanceTypes([AsParameters] DanceTypeSearchRequest request, IDanceTypeService danceTypeService, CancellationToken cancellationToken)
    {
        var danceTypes = await danceTypeService.SearchDanceTypes(request, cancellationToken);

        return TypedResults.Ok(danceTypes);
    }
}
