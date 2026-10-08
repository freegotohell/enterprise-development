using AutoService.Domain.Data;
using AutoService.Domain.Entities;
using AutoService.Domain.Interfaces.Repositories;

namespace AutoService.Infrastructure.InMemory.Repositories;

/// <summary>
/// Repository for client data operations
/// </summary>
public class ClientRepository : IClientRepository
{
    private readonly AutoServiceContext _context;

    /// <summary>
    /// Initializes a new instance of the repo
    /// </summary>
    public ClientRepository(AutoServiceContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public Task<List<Client>> GetAllAsync()
    {
        return Task.FromResult(_context.Clients.ToList());
    }

    /// <inheritdoc />
    public Task<Client?> GetByIdAsync(int id)
    {
        var client = _context.Clients.FirstOrDefault(x => x.Id == id);
        return Task.FromResult(client);
    }

    /// <inheritdoc />
    public Task AddAsync(Client entity)
    {
        _context.Clients.Add(entity);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task UpdateAsync(Client entity)
    {
        var existing = _context.Clients.FirstOrDefault(x => x.Id == entity.Id);

        if (existing is not null)
        {
            var index = _context.Clients.IndexOf(existing);
            _context.Clients[index] = entity;
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task DeleteAsync(Client entity)
    {
        _context.Clients.Remove(entity);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task<bool> HasCarsAsync(int clientId)
    {
        var hasCars = _context.Cars.Any(car => car.ClientId == clientId);
        return Task.FromResult(hasCars);
    }

    /// <inheritdoc />
    public Task<bool> HasOrdersAsync(int clientId)
    {
        var hasOrders = _context.RepairOrders.Any(order => order.ClientId == clientId);
        return Task.FromResult(hasOrders);
    }
}