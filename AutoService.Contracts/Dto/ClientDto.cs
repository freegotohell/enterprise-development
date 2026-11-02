namespace AutoService.Contracts.Dto;

/// <summary>
/// Represents a client returned by API
/// </summary>
public class ClientDto
{
    /// <summary>
    /// Unique id of the client
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Full name of the client
    /// </summary>
    public required string FullName { get; init; }

    /// <summary>
    /// Phone number of the client
    /// </summary>
    public required string Phone { get; init; }
}