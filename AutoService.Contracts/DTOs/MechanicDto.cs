using AutoService.Contracts.Enums;

namespace AutoService.Contracts.DTOs;

/// <summary>
/// Mechanic returned by API
/// </summary>
public class MechanicDto
{
    /// <summary>
    /// Unique id of the mechanic
    /// </summary>
    public int Id { get; set; }

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