namespace AutoService.Contracts.DTOs;

/// <summary>
/// Client with multiple repair orders during the last month
/// </summary>
public class RepeatedClientDto
{
    /// <summary>
    /// Unique id of the client
    /// </summary>
    public int ClientId { get; set; }

    /// <summary>
    /// Number of repair orders during the last month
    /// </summary>
    public required int RequestsCount { get; set; }
}