using System.ComponentModel.DataAnnotations;

namespace AutoService.Contracts.DTOs;

/// <summary>
/// Data required to update a client
/// </summary>
public class UpdateClientDto
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