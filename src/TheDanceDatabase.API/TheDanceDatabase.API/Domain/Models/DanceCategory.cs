namespace TheDanceDatabase.API.Domain.Models;

/// <summary>A group of moves within a dance style, such as Hip Hop's Foot Work.</summary>
public record DanceCategory(
    int Id,
    string Name,
    IReadOnlyList<DanceMove> Moves);
