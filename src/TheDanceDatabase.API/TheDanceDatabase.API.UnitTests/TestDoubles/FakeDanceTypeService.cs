using TheDanceDatabase.API.Application;
using TheDanceDatabase.API.Domain.Models;

namespace TheDanceDatabase.API.UnitTests.TestDoubles;

public class FakeDanceTypeService : IDanceTypeService
{
    public static readonly DanceType FakeDanceType = new(99, "Fake Dance", [new(99, "Fake Category", [new(99, "Fake Move", "Only exists in tests.")])]);

    public static readonly DanceType FakeSearchedDanceType = new(98, "Fake Searched Dance", []);

    public Task<IReadOnlyList<DanceType>> GetDanceTypes(CancellationToken cancellationToken)
        => Task.FromResult<IReadOnlyList<DanceType>>([FakeDanceType]);

    public Task<IReadOnlyList<DanceType>> SearchDanceTypes(DanceTypeSearchRequest request, CancellationToken cancellationToken)
        => Task.FromResult<IReadOnlyList<DanceType>>([FakeSearchedDanceType]);
}
