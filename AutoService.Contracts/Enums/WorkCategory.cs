namespace AutoService.Contracts.Enums;

/// <summary>
/// Category of repair work in API contract
/// </summary>
public enum WorkCategory
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
    /// Vehicle body repair
    /// </summary>
    BodyRepair = 4,

    /// <summary>
    /// General vehicle maintenance
    /// </summary>
    Maintenance = 5
}
