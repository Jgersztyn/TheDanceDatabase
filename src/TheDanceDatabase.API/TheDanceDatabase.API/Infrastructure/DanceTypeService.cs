using TheDanceDatabase.API.Application;
using TheDanceDatabase.API.Domain.Enums;
using TheDanceDatabase.API.Domain.Models;

namespace TheDanceDatabase.API.Infrastructure;

public class DanceTypeService : IDanceTypeService
{
    // TODO: Replace the dummy data with a persisted store
    private static readonly IReadOnlyList<DanceType> DanceTypes =
    [
        new(1, "Swing", DanceCategory.Social, "An energetic partner dance that grew up alongside swing-era jazz."),
        new(2, "Jazz", DanceCategory.Theatrical, "A performance style built on isolations, improvisation and syncopated rhythm."),
        new(3, "Salsa", DanceCategory.Latin, "A Latin partner dance with Cuban roots, danced to an eight-count rhythm.")
    ];

    private readonly ILogger<DanceTypeService> _logger;

    public DanceTypeService(ILogger<DanceTypeService> logger)
    {
        _logger = logger;
    }

    public Task<IReadOnlyList<DanceType>> GetDanceTypes(CancellationToken cancellationToken)
    {
        _logger.LogDebug("Retrieved {DanceTypeCount} dance types", DanceTypes.Count);

        return Task.FromResult(DanceTypes);
    }
}
