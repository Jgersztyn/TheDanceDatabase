using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using TheDanceDatabase.API.Application;
using TheDanceDatabase.API.Domain.Models;

namespace TheDanceDatabase.API.Infrastructure;

public class DanceTypeService : IDanceTypeService
{
    // Matches the LogicalName of the embedded resource in the csproj
    private const string MockDataResourceName = "dance-moves.json";

    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private readonly ILogger<DanceTypeService> _logger;

    public DanceTypeService(ILogger<DanceTypeService> logger)
    {
        _logger = logger;
    }

    /// <summary>Retrieves every dance type from the mock data.</summary>
    public async Task<IReadOnlyList<DanceType>> GetDanceTypes(CancellationToken cancellationToken)
    {
        var danceTypes = await LoadDanceTypes(cancellationToken);

        _logger.LogDebug("Retrieved {DanceTypeCount} dance types", danceTypes.Count);

        return danceTypes;
    }

    /// <summary>Filters the mock data by exact style and category names and part of a move name, pruning branches the filters empty.</summary>
    public async Task<IReadOnlyList<DanceType>> SearchDanceTypes(DanceTypeSearchRequest request, CancellationToken cancellationToken)
    {
        var danceTypes = await LoadDanceTypes(cancellationToken);

        // Only prune styles a category or move filter emptied, so no filters returns every style
        var pruneEmptyStyles = HasTerm(request.Category) || HasTerm(request.Move);

        var results = danceTypes
            .Where(x => IsExactMatch(x.Name, request.Style))
            .Select(x => x with { Categories = FilterCategories(x.Categories, request) })
            .Where(x => !pruneEmptyStyles || x.Categories.Count > 0)
            .ToList();

        // The search terms are user input, so only the count is logged
        _logger.LogDebug("Found {DanceTypeCount} dance types matching the search", results.Count);

        return results;
    }

    private static List<DanceCategory> FilterCategories(IReadOnlyList<DanceCategory> categories, DanceTypeSearchRequest request)
    {
        // Categories with no moves in the data, e.g. Tracing, stay listed unless a move filter is given
        var pruneEmptyCategories = HasTerm(request.Move);

        return categories
            .Where(x => IsExactMatch(x.Name, request.Category))
            .Select(x => x with { Moves = x.Moves.Where(move => IsPartialMatch(move.Name, request.Move)).ToList() })
            .Where(x => !pruneEmptyCategories || x.Moves.Count > 0)
            .ToList();
    }

    // TODO: Replace the mock data with a persisted store
    private static async Task<IReadOnlyList<DanceType>> LoadDanceTypes(CancellationToken cancellationToken)
    {
        await using var stream = typeof(DanceTypeService).Assembly.GetManifestResourceStream(MockDataResourceName)
            ?? throw new InvalidOperationException($"Embedded resource '{MockDataResourceName}' was not found.");

        var mockData = await JsonSerializer.DeserializeAsync<MockDanceData>(stream, JsonOptions, cancellationToken);

        if (mockData?.Styles is null)
        {
            throw new InvalidOperationException($"Embedded resource '{MockDataResourceName}' has no styles.");
        }

        return mockData.Styles;
    }

    private static bool HasTerm([NotNullWhen(true)] string? term)
        => !string.IsNullOrWhiteSpace(term);

    private static bool IsExactMatch(string name, string? term)
        => !HasTerm(term) || name.Equals(term.Trim(), StringComparison.OrdinalIgnoreCase);

    private static bool IsPartialMatch(string name, string? term)
        => !HasTerm(term) || name.Contains(term.Trim(), StringComparison.OrdinalIgnoreCase);

    // The JSON root wraps the styles in an object
    private sealed record MockDanceData(IReadOnlyList<DanceType> Styles);
}
