using TheDanceDatabase.API.Domain.Enums;

namespace TheDanceDatabase.API.Domain.Models;

/// <summary>A style of dance, such as Swing or Salsa.</summary>
public record DanceType(
    int Id,
    string Name,
    DanceCategory Category,
    string Description);
