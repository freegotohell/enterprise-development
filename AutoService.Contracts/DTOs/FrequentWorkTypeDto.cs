namespace AutoService.Contracts.DTOs;

/// <summary>
/// Work type with the number of times it was performed
/// </summary>
public class FrequentWorkTypeDto
{
    /// <summary>
    /// Unique id of the type
    /// </summary>
    public int WorkTypeId { get; set; }

    /// <summary>
    /// Name of the work type
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Number of times the work type was performed
    /// </summary>
    public required int Count { get; set; }
}