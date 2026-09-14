namespace AutoService.Domain.Entities;

/// <summary>
/// Represents a repair order for a client's car.
/// </summary>
public class RepairOrder
{
    /// <summary>
    /// Unique identifier of the repair order.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Identifier of the client associated with the repair order.
    /// </summary>
    public int ClientId { get; set; }

    /// <summary>
    /// Client associated with the repair order.
    /// </summary>
    public Client Client { get; set; } = null!;

    /// <summary>
    /// Identifier of the car associated with the repair order.
    /// </summary>
    public int CarId { get; set; }

    /// <summary>
    /// Car associated with the repair order.
    /// </summary>
    public Car Car { get; set; } = null!;

    /// <summary>
    /// Date and time when the car was admitted for repair.
    /// </summary>
    public DateTime AdmissionDate { get; set; }

    /// <summary>
    /// Date and time when the repair order was completed.
    /// </summary>
    public DateTime? ReleaseDate { get; set; }

    /// <summary>
    /// Mechanics assigned to the repair order.
    /// </summary>
    public List<OrderMechanic> Mechanics { get; set; } = [];

    /// <summary>
    /// Works included in the repair order.
    /// </summary>
    public List<OrderWork> Works { get; set; } = [];

}
