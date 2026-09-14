using AutoService.Domain.Entities;

namespace AutoService.Domain.Data;

/// <summary>
/// Represents the in-memory data context of the auto service.
/// </summary>
public class AutoServiceContext
{
    /// <summary>
    /// Collection of clients.
    /// </summary>
    public List<Client> Clients { get; set; } = [];

    /// <summary>
    /// Collection of cars.
    /// </summary>
    public List<Car> Cars { get; set; } = [];

    /// <summary>
    /// Collection of mechanics.
    /// </summary>
    public List<Mechanic> Mechanics { get; set; } = [];

    /// <summary>
    /// Collection of work types.
    /// </summary>
    public List<WorkType> WorkTypes { get; set; } = [];

    /// <summary>
    /// Collection of repair orders.
    /// </summary>
    public List<RepairOrder> RepairOrders { get; set; } = [];
}