using TheDanceDatabase.API.Application;
using TheDanceDatabase.API.Domain.Enums;
using TheDanceDatabase.API.Domain.Models;

namespace TheDanceDatabase.API.UnitTests.TestDoubles;

public class FakeDanceTypeService : IDanceTypeService
{
    public static readonly DanceType FakeDanceType = new(99, "Fake Dance", DanceCategory.Unknown, "A dance type that only exists in tests.");

    public Task<IReadOnlyList<DanceType>> GetDanceTypes(CancellationToken cancellationToken)
        => Task.FromResult<IReadOnlyList<DanceType>>([FakeDanceType]);
}
