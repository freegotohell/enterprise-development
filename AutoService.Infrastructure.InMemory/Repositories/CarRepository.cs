using AutoService.Domain.Data;
using AutoService.Domain.Entities;
using AutoService.Domain.Interfaces.Repositories;

namespace AutoService.Infrastructure.InMemory.Repositories;

/// <summary>
/// Repository for car data operations
/// </summary>
public class CarRepository(AutoServiceContext context) : ICarRepository
{
    private int _nextId = context.Cars.Count == 0 ? 1 : context.Cars.Max(car => car.Id) + 1;

    /// <inheritdoc />
    public Task<List<Car>> GetAllAsync()
    {
        return Task.FromResult(context.Cars.ToList());
    }

    /// <inheritdoc />
    public Task<Car?> GetByIdAsync(int id)
    {
        Car? car = context.Cars.FirstOrDefault(x => x.Id == id);

        return Task.FromResult(car);
    }

    /// <inheritdoc />
    public Task<Car> AddAsync(Car entity)
    {
        if (context.Cars.Contains(entity))
        {
            return Task.FromResult(entity);
        }

        entity.Id = _nextId++;
        context.Cars.Add(entity);
        entity.Client.Cars.Add(entity);

        return Task.FromResult(entity);
    }

    /// <inheritdoc />
    public Task<bool> UpdateAsync(Car entity)
    {
        var index = context.Cars.FindIndex(car => car.Id == entity.Id);

        if (index == -1)
        {
            return Task.FromResult(false);
        }

        foreach (Client client in context.Clients)
        {
            client.Cars.Remove(context.Cars[index]);
        }

        context.Cars[index] = entity;
        entity.Client.Cars.Add(entity);

        return Task.FromResult(true);
    }

    /// <inheritdoc />
    public Task<bool> DeleteAsync(Car entity)
    {
        Car? existing = context.Cars.FirstOrDefault(car => car.Id == entity.Id);

        if (existing is null)
        {
            return Task.FromResult(false);
        }

        context.Cars.Remove(existing);
        existing.Client.Cars.Remove(existing);

        return Task.FromResult(true);
    }

    /// <inheritdoc />
    public Task<List<Car>> GetByClientIdAsync(int clientId)
    {
        return Task.FromResult(context.Cars.Where(car => car.ClientId == clientId).ToList());
    }

    /// <inheritdoc />
    public Task<bool> HasOrdersAsync(int carId)
    {
        var hasOrders = context.RepairOrders.Any(order => order.CarId == carId);

        return Task.FromResult(hasOrders);
    }
}