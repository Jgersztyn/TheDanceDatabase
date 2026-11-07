namespace TheDanceDatabase.API.Domain.Models;

/// <summary>A style of dance, such as Shuffling or Hip Hop, grouped into categories of moves.</summary>
public record DanceType(
    int Id,
    string Name,
    IReadOnlyList<DanceCategory> Categories);
