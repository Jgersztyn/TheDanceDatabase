using Microsoft.Extensions.DependencyInjection;
using TheDanceDatabase.API.Application;
using TheDanceDatabase.API.Presentation;
using TheDanceDatabase.API.UnitTests.TestDoubles;

namespace TheDanceDatabase.API.UnitTests.Presentation;

public class FunctionsTests : FunctionsTest
{
    [Fact]
    public async Task GetDanceTypes_ShouldReturnServiceResult_IfServiceReturnsDanceTypes()
    {
        // Arrange
        await using var scope = Provider.CreateAsyncScope();
        var danceTypeService = scope.ServiceProvider.GetRequiredService<IDanceTypeService>();

        // Act
        var result = await Functions.GetDanceTypes(danceTypeService, TestContext.Current.CancellationToken);

        // Assert
        result.Value.ShouldBe([FakeDanceTypeService.FakeDanceType]);
    }
}
