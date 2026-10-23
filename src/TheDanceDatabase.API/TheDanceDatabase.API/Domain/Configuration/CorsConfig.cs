namespace TheDanceDatabase.API.Domain.Configuration;

/// <summary>Browser origins allowed to call the API, such as the UI dev server.</summary>
public class CorsConfig
{
    public string[] AllowedOrigins { get; set; } = [];
}
