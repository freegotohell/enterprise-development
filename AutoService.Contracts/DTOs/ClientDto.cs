namespace AutoService.Contracts.DTOs;

/// <summary>
/// Represents a client returned by API
/// </summary>
public class ClientDto
{
    /// <summary>
    /// Unique id of the client
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Full name of the client
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    /// Phone number of the client
    /// </summary>
    public required string Phone { get; set; }
}