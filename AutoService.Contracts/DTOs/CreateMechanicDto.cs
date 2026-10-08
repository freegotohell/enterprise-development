using AutoService.Domain.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace AutoService.Contracts.DTOs;

/// <summary>
/// Data required to create a mechanic
/// </summary>
public class CreateMechanicDto
{
    /// <summary>
    /// Passport number of the mechanic
    /// </summary>
    [Required]
    [StringLength(10)]
    public required string PassportNumber { get; set; }

    /// <summary>
    /// Full name of the mechanic
    /// </summary>
    [Required]
    public required string FullName { get; set; }

    /// <summary>
    /// Specialization of the mechanic
    /// </summary>
    [Range(0, 4)]
    public MechanicSpecialization Specialization { get; set; }

    /// <summary>
    /// Work experience of the mechanic in years
    /// </summary>
    [Range(0, 60)]
    public int Experience { get; set; }
}