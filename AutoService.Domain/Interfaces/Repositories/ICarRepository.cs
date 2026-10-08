using AutoService.Domain.Entities;

namespace AutoService.Domain.Interfaces.Repositories;

/// <summary>
/// Data access operations for cars
/// </summary>
public interface ICarRepository : IRepository<Car>
{
    /// <summary>
    /// Gets cars assosiated with the client
    /// </summary>
    public Task<List<Car>> GetByClientIdAsync(int clientId);

    /// <summary>
    /// Checks if the car has repair orders
    /// </summary>
    public Task<bool> HasOrdersAsync(int carId);
}