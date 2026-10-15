using AutoService.Domain.Data;
using AutoService.Domain.Entities;
using AutoService.Domain.Interfaces.Repositories;

namespace AutoService.Infrastructure.InMemory.Repositories;

/// <summary>
/// Repository for client data operations
/// </summary>
public class ClientRepository(AutoServiceContext context) : IClientRepository
{
    /// <inheritdoc />
    public Task<List<Client>> GetAllAsync()
    {
        return Task.FromResult(context.Clients.ToList());
    }

    /// <inheritdoc />
    public Task<Client?> GetByIdAsync(int id)
    {
        var client = context.Clients.FirstOrDefault(x => x.Id == id);
        
        return Task.FromResult(client);
    }

    /// <inheritdoc />
    public Task<Client> AddAsync(Client entity)
    {
        if (context.Clients.Contains(entity))
        {
            return Task.FromResult(entity);
        }

        entity.Id = context.Clients.Count == 0 ? 1 : context.Clients.Max(client => client.Id) + 1;
        context.Clients.Add(entity);

        return Task.FromResult(entity);
    }

    /// <inheritdoc />
    public Task<bool> UpdateAsync(Client entity)
    {
        var index = context.Clients.FindIndex(client => client.Id == entity.Id);

        if (index == -1)
        {
            return Task.FromResult(false);
        }

        context.Clients[index] = entity;

        return Task.FromResult(true);
    }

    /// <inheritdoc />
    public Task<bool> DeleteAsync(Client entity)
    {
        var existing = context.Clients.FirstOrDefault(client => client.Id == entity.Id);

        if (existing is null)
        {
            return Task.FromResult(false);
        }

        context.Clients.Remove(existing);

        return Task.FromResult(true);
    }

    /// <inheritdoc />
    public Task<bool> HasCarsAsync(int clientId)
    {
        var hasCars = context.Cars.Any(car => car.ClientId == clientId);
        
        return Task.FromResult(hasCars);
    }

    /// <inheritdoc />
    public Task<bool> HasOrdersAsync(int clientId)
    {
        var hasOrders = context.RepairOrders.Any(order => order.ClientId == clientId);
        
        return Task.FromResult(hasOrders);
    }
}