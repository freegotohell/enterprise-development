using AutoService.Domain.Enums;

namespace AutoService.Domain.Entities;

/// <summary>
/// Represents a mechanic working at the auto service.
/// </summary>
public class Mechanic
{
    /// <summary>
    /// Unique identifier of the mechanic.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Passport number of the mechanic.
    /// </summary>
    public required string PassportNumber { get; set; }

    /// <summary>
    /// Full name of the mechanic.
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    /// Specialization of the mechanic.
    /// </summary>
    public MechanicSpecialization Specialization { get; set; }

    /// <summary>
    /// Work experience of the mechanic in years.
    /// </summary>
    public int Experience { get; set; }

    /// <summary>
    /// Repair orders assigned to the mechanic.
    /// </summary>
    public List<OrderMechanic> Orders { get; set; } = [];

}
