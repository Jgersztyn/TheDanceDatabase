using TheDanceDatabase.API.Domain.Models;

namespace TheDanceDatabase.API.Application;

public interface IDanceTypeService
{
    /// <summary>Retrieves every supported dance type.</summary>
    Task<IReadOnlyList<DanceType>> GetDanceTypes(CancellationToken cancellationToken);

    /// <summary>Retrieves the dance types, categories and moves that match the request's filters.</summary>
    Task<IReadOnlyList<DanceType>> SearchDanceTypes(DanceTypeSearchRequest request, CancellationToken cancellationToken);
}
