using System.Text.Json.Serialization;
using TheDanceDatabase.API.Application;
using TheDanceDatabase.API.Domain.Configuration;
using TheDanceDatabase.API.Infrastructure;

namespace TheDanceDatabase.API;

public static class DependencyInjection
{
    /// <summary>Registers the clock and the application services implemented in Infrastructure.</summary>
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Single source of truth for the current UTC time; inject TimeProvider rather than calling DateTime.UtcNow
        services.AddSingleton(TimeProvider.System);

        services.AddScoped<IDanceTypeService, DanceTypeService>();

        return services;
    }

    /// <summary>Registers HTTP concerns: OpenAPI, problem details, health checks, JSON and CORS.</summary>
    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        var corsConfig = new CorsConfig();

        configuration.Bind(nameof(CorsConfig), corsConfig);
        services.AddSingleton(corsConfig);

        services.AddOpenApi();
        services.AddProblemDetails();
        services.AddHealthChecks();

        // Serialize enums by name so callers receive "Latin" rather than 2
        services.ConfigureHttpJsonOptions(options =>
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        services.AddCors(options => options.AddDefaultPolicy(policy =>
            policy.WithOrigins(corsConfig.AllowedOrigins)
                .AllowAnyMethod()
                .AllowAnyHeader()));

        return services;
    }
}
