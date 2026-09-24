using AutoService.Contracts.Enums;

namespace AutoService.Contracts.DTOs;

/// <summary>
/// Type of repair work provided by the auto service returned by API
/// </summary>
public class WorkTypeDto
{
    /// <summary>
    /// Unique id of the work type
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name of the work type
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Category of the work
    /// </summary>
    public WorkCategory Category { get; set; }

    /// <summary>
    /// Price of the work
    /// </summary>
    public decimal Cost { get; set; }

    /// <summary>
    /// Estimated duration of the work
    /// </summary>
    public TimeSpan Duration { get; set; }

    /// <summary>
    /// Description of the work
    /// </summary>
    public required string Description { get; set; }
}