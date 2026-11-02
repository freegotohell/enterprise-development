using AutoService.Domain.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace AutoService.Contracts.Dto;

/// <summary>
/// Data required to create or update a work type
/// </summary>
public class UpsertWorkTypeDto
{
    /// <summary>
    /// Name of the work type
    /// </summary>
    [Required]
    public required string Name { get; init; }

    /// <summary>
    /// Category of the work
    /// </summary>
    [Range(0, 5)]
    public WorkCategory Category { get; init; }

    /// <summary>
    /// Price of the work
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal Cost { get; init; }

    /// <summary>
    /// Estimated duration of the work
    /// </summary>
    [Range(typeof(TimeSpan), "00:00:00", "90.00:00:00")]
    public TimeSpan Duration { get; init; }

    /// <summary>
    /// Description of the work
    /// </summary>
    [Required]
    public required string Description { get; init; }
}