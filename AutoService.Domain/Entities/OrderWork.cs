namespace AutoService.Domain.Entities;

/// <summary>
/// Represents the association between a repair order and a work type.
/// </summary>
public class OrderWork
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
    /// Identifier of the associated work type.
    /// </summary>
    public int WorkTypeId { get; set; }

    /// <summary>
    /// Associated work type.
    /// </summary>
    public WorkType WorkType { get; set; } = null!;

}
