namespace AutoService.Contracts.Dto;

/// <summary>
/// Car returned by the API
/// </summary>
public class CarDto
{
    /// <summary>
    /// Unique id of the car
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// License plate number
    /// </summary>
    public required string LicensePlate { get; init; }

    /// <summary>
    /// Car brand
    /// </summary>
    public required string Brand { get; init; }

    /// <summary>
    /// Car model
    /// </summary>
    public required string Model { get; init; }

    /// <summary>
    /// Car production year
    /// </summary>
    public int? Year { get; init; }

    /// <summary>
    /// Id of the client who owns the car
    /// </summary>
    public int ClientId { get; init; }
}