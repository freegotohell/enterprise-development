namespace AutoService.Contracts.DTOs;

/// <summary>
/// Data required to create a client
/// </summary>
public class CreateClientDto
{
    /// <summary>
    /// Full name of the client
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    /// Phone number of the client
    /// </summary>
    public required string Phone { get; set; }
}