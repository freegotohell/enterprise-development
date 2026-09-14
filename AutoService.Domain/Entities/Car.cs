namespace AutoService.Domain.Entities;

/// <summary>
/// Represents a car registered to a client.
/// </summary>
public class Car
{

    /// <summary>
    /// Unique identifier of the car.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// License plate number.
    /// </summary>
    public required string LicensePlate { get; set; }

    /// <summary>
    /// Car brand.
    /// </summary>
    public required string Brand { get; set; }

    /// <summary>
    /// Car model.
    /// </summary>
    public required string Model { get; set; }

    /// <summary>
    /// Car production year.
    /// </summary>
    public int? Year { get; set; }

    /// <summary>
    /// Identifier of the client who owns the car.
    /// </summary>
    public int ClientId { get; set; }

    /// <summary>
    /// Client who owns the car.
    /// </summary>
    public Client Client { get; set; } = null!;

    /// <summary>
    /// Repair orders associated with the car.
    /// </summary>
    public List<RepairOrder> Orders { get; set; } = [];
}