namespace AutoService.Contracts.Dto;

/// <summary>
/// Total cost of a repair order
/// </summary>
public class RepairOrderCostDto
{
    /// <summary>
    /// Unique id of the repair order
    /// </summary>
    public int RepairOrderId { get; init; }

    /// <summary>
    /// Total cost of all works in the repair order
    /// </summary>
    public required decimal TotalCost { get; init; }
}