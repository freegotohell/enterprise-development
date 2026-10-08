using AutoService.Domain.Data;
using AutoService.Domain.Entities;
using AutoService.Domain.Interfaces.Repositories;

namespace AutoService.Infrastructure.InMemory.Repositories;

/// <summary>
/// Repository for car data operations
/// </summary>
public class CarRepository : ICarRepository
{
    private readonly AutoServiceContext _context;

    /// <summary>
    /// Initializes a new instance of the repo
    /// </summary>
    public CarRepository(AutoServiceContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public Task<List<Car>> GetAllAsync()
    {
        return Task.FromResult(_context.Cars.ToList());
    }

    /// <inheritdoc />
    public Task<Car?> GetByIdAsync(int id)
    {
        var car = _context.Cars.FirstOrDefault(x => x.Id == id);

        return Task.FromResult(car);
    }

    /// <inheritdoc />
    public Task AddAsync(Car entity)
    {
        _context.Cars.Add(entity);

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task UpdateAsync(Car entity)
    {
        var existing = _context.Cars.FirstOrDefault(x => x.Id == entity.Id);

        if (existing is not null)
        {
            var index = _context.Cars.IndexOf(existing);
            _context.Cars[index] = entity;
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task DeleteAsync(Car entity)
    {
        _context.Cars.Remove(entity);

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task<List<Car>> GetByClientIdAsync(int clientId)
    {
        return Task.FromResult(_context.Cars.Where(car => car.ClientId == clientId).ToList());
    }

    /// <inheritdoc />
    public Task<bool> HasOrdersAsync(int carId)
    {
        var hasOrders = _context.RepairOrders.Any(order => order.CarId == carId);

        return Task.FromResult(hasOrders);
    }
}