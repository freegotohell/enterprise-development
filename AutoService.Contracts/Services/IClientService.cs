using AutoService.Contracts.DTOs;
using AutoService.Domain.Shared.Results;

namespace AutoService.Contracts.Services;

/// <summary>
/// Operations for managing clients
/// </summary>
public interface IClientService
{
    /// <summary>
    /// Gets all clients
    /// </summary>
    public Task<List<ClientDto>> GetAllAsync();

    /// <summary>
    /// Gets a client by id
    /// </summary>
    public Task<ClientDto?> GetByIdAsync(int id);

    /// <summary>
    /// Gets clients with more than one repair order during the last month
    /// </summary>
    public Task<List<RepeatedClientDto>> GetRepeatedLastMonthAsync();

    /// <summary>
    /// Creates a new client
    /// </summary>
    public Task<ClientDto> CreateAsync(CreateClientDto dto);

    /// <summary>
    /// Updates an existing client
    /// </summary>
    public Task<ClientDto?> UpdateAsync(int id, UpdateClientDto dto);

    /// <summary>
    /// Deletes a client
    /// </summary>
    public Task<DeleteResult> DeleteAsync(int id);
}