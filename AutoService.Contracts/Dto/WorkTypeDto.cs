using AutoService.Domain.Shared.Enums;

namespace AutoService.Contracts.Dto;

/// <summary>
/// Type of repair work provided by the auto service returned by API
/// </summary>
public class WorkTypeDto
{
    /// <summary>
    /// Unique id of the work type
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Name of the work type
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Category of the work
    /// </summary>
    public WorkCategory Category { get; init; }

    /// <summary>
    /// Price of the work
    /// </summary>
    public decimal Cost { get; init; }

    /// <summary>
    /// Estimated duration of the work
    /// </summary>
    public TimeSpan Duration { get; init; }

    /// <summary>
    /// Description of the work
    /// </summary>
    public required string Description { get; init; }
}