namespace AutoService.Contracts.DTOs;

/// <summary>
/// Data required to create a car
/// </summary>
public class CreateCarDto
{
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