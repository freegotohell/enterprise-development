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
    public required string FullName { get; set; }

    /// <summary>
    /// Phone number of the client
    /// </summary>
    [Required]
    public required string Phone { get; set; }
}