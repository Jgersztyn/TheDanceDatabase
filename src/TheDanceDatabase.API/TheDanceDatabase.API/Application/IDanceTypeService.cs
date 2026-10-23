using TheDanceDatabase.API.Domain.Models;

namespace TheDanceDatabase.API.Application;

public interface IDanceTypeService
{
    /// <summary>Retrieves every supported dance type.</summary>
    Task<IReadOnlyList<DanceType>> GetDanceTypes(CancellationToken cancellationToken);
}
