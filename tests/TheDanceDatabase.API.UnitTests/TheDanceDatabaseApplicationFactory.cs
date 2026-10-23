using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace TheDanceDatabase.API.UnitTests;

/// <summary>Hosts the API in memory with pinned settings, so tests never read developer-local files.</summary>
public class TheDanceDatabaseApplicationFactory : WebApplicationFactory<Program>
{
    public const string AllowedOrigin = "http://localhost:3000";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Not Development, so appsettings.Development.json and appsettings.Local.json are skipped
        builder.UseEnvironment("Testing");
        builder.UseSetting("CorsConfig:AllowedOrigins:0", AllowedOrigin);

        // Keep the startup scope and registration checks that Development turns on
        builder.UseDefaultServiceProvider(options =>
        {
            options.ValidateScopes = true;
            options.ValidateOnBuild = true;
        });
    }
}
