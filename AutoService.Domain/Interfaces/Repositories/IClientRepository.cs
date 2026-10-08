using AutoService.Domain.Entities;

namespace AutoService.Domain.Interfaces.Repositories;

/// <summary>
/// Data access operations for clients
/// </summary>
public interface IClientRepository : IRepository<Client>
{
    /// <summary>
    /// Checks if the client has cars assigned to him
    /// </summary>
    public Task<bool> HasCarsAsync(int clientId);

    /// <summary>
    /// Checks if the client has placed orders
    /// </summary>
    public Task<bool> HasOrdersAsync(int clientId);
}