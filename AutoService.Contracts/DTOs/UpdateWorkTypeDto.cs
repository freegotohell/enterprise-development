using AutoService.Domain.Shared.Enums;

namespace AutoService.Contracts.DTOs;

/// <summary>
/// Data required to update a work type
/// </summary>
public class UpdateWorkTypeDto
{
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