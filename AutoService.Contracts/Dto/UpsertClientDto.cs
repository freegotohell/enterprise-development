using System.ComponentModel.DataAnnotations;

namespace AutoService.Contracts.Dto;

/// <summary>
/// Data required to create or update a client
/// </summary>
public class UpsertClientDto
{
    /// <summary>
    /// Full name of the client
    /// </summary>
    [Required]
    [StringLength(200)]
    public required string FullName { get; init; }

    /// <summary>
    /// Phone number of the client
    /// </summary>
    [Required]
    [Phone]
    [StringLength(32)]
    public required string Phone { get; init; }
}