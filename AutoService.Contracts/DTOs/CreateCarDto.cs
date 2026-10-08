using System.ComponentModel.DataAnnotations;

namespace AutoService.Contracts.DTOs;

/// <summary>
/// Data required to create a car
/// </summary>
public class CreateCarDto
{
    /// <summary>
    /// License plate number
    /// </summary>
    [Required]
    public required string LicensePlate { get; set; }

    /// <summary>
    /// Car brand
    /// </summary>
    [Required]
    public required string Brand { get; set; }

    /// <summary>
    /// Car model
    /// </summary>
    [Required]
    public required string Model { get; set; }

    /// <summary>
    /// Car production year
    /// </summary>
    [Range(1900, 2100)]
    public int? Year { get; set; }

    /// <summary>
    /// Id of the client who owns the car
    /// </summary>
    [Range(1, int.MaxValue)]
    public int ClientId { get; set; }
}