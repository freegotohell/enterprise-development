namespace AutoService.Domain.Entities;

/// <summary>
/// Represents a client of the auto service.
/// </summary>
public class Client
{
    /// <summary>
    /// Unique identifier of the client.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Full name of the client.
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    /// Phone number of the client.
    /// </summary>
    public required string Phone { get; set; }

    /// <summary>
    /// Cars owned by the client.
    /// </summary>
    public List<Car> Cars { get; set; } = [];

    /// <summary>
    /// Repair orders associated with the client.
    /// </summary>
    public List<RepairOrder> Orders { get; set; } = [];

}
