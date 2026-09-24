namespace AutoService.Contracts.DTOs;

/// <summary>
/// Car returned by the API
/// </summary>
public class CarDto
{
    /// <summary>
    /// Unique id of the car
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// License plate number
    /// </summary>
    public required string LicensePlate { get; set; }

    /// <summary>
    /// Car brand
    /// </summary>
    public required string Brand { get; set; }

    /// <summary>
    /// Car model
    /// </summary>
    public required string Model { get; set; }

    /// <summary>
    /// Car production year
    /// </summary>
    public int? Year { get; set; }

    /// <summary>
    /// Id of the client who owns the car
    /// </summary>
    public int ClientId { get; set; }
}