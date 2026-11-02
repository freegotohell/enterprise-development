using System.ComponentModel.DataAnnotations;

namespace AutoService.Contracts.Dto;

/// <summary>
/// Data required to create or update a car
/// </summary>
public class UpsertCarDto
{
    /// <summary>
    /// License plate number
    /// </summary>
    [Required]
    public required string LicensePlate { get; init; }

    /// <summary>
    /// Car brand
    /// </summary>
    [Required]
    public required string Brand { get; init; }

    /// <summary>
    /// Car model
    /// </summary>
    [Required]
    public required string Model { get; init; }

    /// <summary>
    /// Car production year
    /// </summary>
    [Range(1900, 2100)]
    public int? Year { get; init; }

    /// <summary>
    /// Id of the client who owns the car
    /// </summary>
    [Range(1, int.MaxValue)]
    public int ClientId { get; init; }
}