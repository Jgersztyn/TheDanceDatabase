namespace TheDanceDatabase.API.Domain.Models;

/// <summary>An individual dance move, with optional free-text notes.</summary>
public record DanceMove(
    int Id,
    string Name,
    string? Notes);
