using AutoService.Domain.Entities;
using AutoService.Domain.Interfaces.Repositories;
using AutoService.Infrastructure.EfCore.Data;
using Microsoft.EntityFrameworkCore;

namespace AutoService.Infrastructure.EfCore.Repositories;

/// <summary>
/// Repository for client data operations
/// </summary>
public class ClientRepository(AutoServiceDbContext context) : IClientRepository
{
    /// <inheritdoc />
    public async Task<List<Client>> GetAllAsync()
    {
        return await context.Clients.AsNoTracking().ToListAsync();
    }

    /// <inheritdoc />
    public async Task<Client?> GetByIdAsync(int id)
    {
        return await context.Clients.FirstOrDefaultAsync(client => client.Id == id);
    }

    /// <inheritdoc />
    public async Task<Client> AddAsync(Client entity)
    {
        await context.Clients.AddAsync(entity);
        await context.SaveChangesAsync();

        return entity;
    }

    /// <inheritdoc />
    public async Task<bool> UpdateAsync(Client entity)
    {
        context.Clients.Update(entity);

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
    public async Task<bool> DeleteAsync(Client entity)
    {
        context.Clients.Remove(entity);

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
    public Task<bool> HasCarsAsync(int clientId)
    {
        return context.Cars.AnyAsync(car => car.ClientId == clientId);
    }

    /// <inheritdoc />
    public Task<bool> HasOrdersAsync(int clientId)
    {
        return context.RepairOrders.AnyAsync(order => order.ClientId == clientId);
    }
}
