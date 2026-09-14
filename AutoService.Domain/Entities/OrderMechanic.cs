namespace AutoService.Domain.Entities;

/// <summary>
/// Represents the association between a repair order and a mechanic.
/// </summary>
public class OrderMechanic
{
    /// <summary>
    /// Unique identifier of the association.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Identifier of the associated repair order.
    /// </summary>
    public int RepairOrderId { get; set; }

    /// <summary>
    /// Associated repair order.
    /// </summary>
    public RepairOrder RepairOrder { get; set; } = null!;

    /// <summary>
    /// Identifier of the associated mechanic.
    /// </summary>
    public int MechanicId { get; set; }

    /// <summary>
    /// Associated mechanic.
    /// </summary>
    public Mechanic Mechanic { get; set; } = null!;

}
