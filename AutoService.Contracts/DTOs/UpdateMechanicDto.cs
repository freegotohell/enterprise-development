using AutoService.Contracts.Enums;

namespace AutoService.Contracts.DTOs;

/// <summary>
/// Data required to update a mechanic
/// </summary>
public class UpdateMechanicDto
{
    /// <summary>
    /// Passport number of the mechanic
    /// </summary>
    public required string PassportNumber { get; set; }

    /// <summary>
    /// Full name of the mechanic
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    /// Specialization of the mechanic
    /// </summary>
    public MechanicSpecialization Specialization { get; set; }

    /// <summary>
    /// Work experience of the mechanic in years
    /// </summary>
    public int Experience { get; set; }
}