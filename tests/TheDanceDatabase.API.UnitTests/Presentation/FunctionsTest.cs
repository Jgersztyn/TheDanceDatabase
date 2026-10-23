using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TheDanceDatabase.API.Application;
using TheDanceDatabase.API.UnitTests.TestDoubles;

namespace TheDanceDatabase.API.UnitTests.Presentation;

/// <summary>Swaps the real services for fakes so handlers are tested in isolation.</summary>
public class FunctionsTest : TheDanceDatabaseTest
{
    protected override void ConfigureServices()
    {
        Services.RemoveAll<IDanceTypeService>();
        Services.AddScoped<IDanceTypeService, FakeDanceTypeService>();
    }
}
