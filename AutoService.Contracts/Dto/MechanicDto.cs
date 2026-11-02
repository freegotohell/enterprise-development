using AutoService.Domain.Shared.Enums;

namespace AutoService.Contracts.Dto;

/// <summary>
/// Mechanic returned by API
/// </summary>
public class MechanicDto
{
    /// <summary>
    /// Unique id of the mechanic
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Passport number of the mechanic
    /// </summary>
    public required string PassportNumber { get; init; }

    /// <summary>
    /// Full name of the mechanic
    /// </summary>
    public required string FullName { get; init; }

    /// <summary>
    /// Specialization of the mechanic
    /// </summary>
    public MechanicSpecialization Specialization { get; init; }

    /// <summary>
    /// Work experience of the mechanic in years
    /// </summary>
    public int Experience { get; init; }
}