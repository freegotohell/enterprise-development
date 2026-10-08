using AutoService.Domain.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace AutoService.Contracts.DTOs;

/// <summary>
/// Data required to create a work type
/// </summary>
public class CreateWorkTypeDto
{
    /// <summary>
    /// Name of the work type
    /// </summary>
    [Required]
    public required string Name { get; set; }

    /// <summary>
    /// Category of the work
    /// </summary>
    [Range(0, 5)]
    public WorkCategory Category { get; set; }

    /// <summary>
    /// Price of the work
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal Cost { get; set; }

    /// <summary>
    /// Estimated duration of the work
    /// </summary>
    public TimeSpan Duration { get; set; }

    /// <summary>
    /// Description of the work
    /// </summary>
    [Required]
    public required string Description { get; set; }
}