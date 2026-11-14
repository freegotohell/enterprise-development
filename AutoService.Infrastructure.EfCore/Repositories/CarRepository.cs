using AutoService.Domain.Entities;
using AutoService.Domain.Interfaces.Repositories;
using AutoService.Infrastructure.EfCore.Data;
using Microsoft.EntityFrameworkCore;

namespace AutoService.Infrastructure.EfCore.Repositories;

/// <summary>
/// Repository for car data operations
/// </summary>
public class CarRepository(AutoServiceDbContext context) : ICarRepository
{
    /// <inheritdoc />
    public async Task<List<Car>> GetAllAsync()
    {
        return await context.Cars.AsNoTracking().ToListAsync();
    }

    /// <inheritdoc />
    public async Task<Car?> GetByIdAsync(int id)
    {
        return await context.Cars.FirstOrDefaultAsync(car => car.Id == id);
    }

    /// <inheritdoc />
    public async Task<Car> AddAsync(Car entity)
    {
        await context.Cars.AddAsync(entity);
        await context.SaveChangesAsync();

        return entity;
    }

    /// <inheritdoc />
    public async Task<bool> UpdateAsync(Car entity)
    {
        context.Cars.Update(entity);

        try
        {
            await context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            return false;
        }
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(Car entity)
    {
        context.Cars.Remove(entity);

        try
        {
            await context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            return false;
        }
    }

    /// <inheritdoc />
    public Task<List<Car>> GetByClientIdAsync(int clientId)
    {
        return context.Cars.AsNoTracking().Where(car => car.ClientId == clientId).ToListAsync();
    }

    /// <inheritdoc />
    public Task<bool> HasOrdersAsync(int carId)
    {
        return context.RepairOrders.AnyAsync(order => order.CarId == carId);
    }
}
