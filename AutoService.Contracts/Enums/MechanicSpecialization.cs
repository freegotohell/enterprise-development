namespace AutoService.Contracts.Enums;

/// <summary>
/// Specialization of a mechanic in API contract
/// </summary>
public enum MechanicSpecialization
{
    /// <summary>
    /// Engine repair and maintenance
    /// </summary>
    Engine = 0,

    /// <summary>
    /// Transmission repair and maintenance
    /// </summary>
    Transmission = 1,

    /// <summary>
    /// Electrical system repair and maintenance
    /// </summary>
    Electrical = 2,

    /// <summary>
    /// Vehicle diagnostics
    /// </summary>
    Diagnostics = 3,

    /// <summary>
    /// Body repair
    /// </summary>
    BodyRepair = 4
}