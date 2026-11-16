using Microsoft.AspNetCore.Mvc;

namespace TheDanceDatabase.API.Application;

/// <summary>Query parameters for searching dance types; a missing or blank value does not filter. Binding ignores case, and OpenAPI documents the lowercase names.</summary>
public class DanceTypeSearchRequest
{
    /// <summary>The exact style name, ignoring case.</summary>
    [FromQuery(Name = "style")]
    public string? Style { get; set; }

    /// <summary>The exact category name, ignoring case.</summary>
    [FromQuery(Name = "category")]
    public string? Category { get; set; }

    /// <summary>Part of a move name, ignoring case.</summary>
    [FromQuery(Name = "move")]
    public string? Move { get; set; }
}
