using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TheDanceDatabase.API.UnitTests;

/// <summary>Builds the application services from <c>AddServices</c> with in-memory configuration for each test class.</summary>
public class TheDanceDatabaseTest : IAsyncLifetime
{
    protected IServiceCollection Services { get; } = new ServiceCollection();

    // InitializeAsync ensures not null
    protected ServiceProvider Provider { get; private set; } = null!;

    public virtual ValueTask InitializeAsync()
    {
        // Add settings here as services start reading configuration
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>())
            .Build();

        Services.AddLogging();
        Services.AddServices(configuration);

        ConfigureServices();

        // Match the Development host so captive scoped dependencies fail here too
        Provider = Services.BuildServiceProvider(new ServiceProviderOptions { ValidateScopes = true, ValidateOnBuild = true });

        return ValueTask.CompletedTask;
    }

    /// <summary>Runs before the provider is built, so feature fixtures can swap real services for fakes.</summary>
    protected virtual void ConfigureServices()
    {
    }

    public virtual async ValueTask DisposeAsync()
    {
        await Provider.DisposeAsync();

        GC.SuppressFinalize(this);
    }
}
