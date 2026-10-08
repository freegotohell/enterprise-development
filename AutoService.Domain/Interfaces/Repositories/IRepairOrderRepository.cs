using AutoService.Domain.Entities;

namespace AutoService.Domain.Interfaces.Repositories;

/// <summary>
/// Data access operations for repair orders
/// </summary>
public interface IRepairOrderRepository : IRepository<RepairOrder>
{
    /// <summary>
    /// Gets repair orders for the client
    /// </summary>
    public Task<List<RepairOrder>> GetByClientIdAsync(int clientId);

    /// <summary>
    /// Gets repair orders for the car
    /// </summary>
    public Task<List<RepairOrder>> GetByCarIdAsync(int carId);

    /// <summary>
    /// Gets repair orders assigned to the mechanic
    /// </summary>
    public Task<List<RepairOrder>> GetByMechanicIdAsync(int mechanicId);

    /// <summary>
    /// Gets repair orders containing the work type
    /// </summary>
    public Task<List<RepairOrder>> GetByWorkTypeIdAsync(int workTypeId);
}