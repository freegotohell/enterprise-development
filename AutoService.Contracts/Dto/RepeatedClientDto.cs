namespace AutoService.Contracts.Dto;

/// <summary>
/// Client with multiple repair orders during the last month
/// </summary>
public class RepeatedClientDto
{
    /// <summary>
    /// Unique id of the client
    /// </summary>
    public int ClientId { get; init; }

    /// <summary>
    /// Number of repair orders during the last month
    /// </summary>
    public required int RequestsCount { get; init; }
}