using AutoService.Contracts.DTOs;
using AutoService.Contracts.Services;
using AutoService.Domain.Entities;
using AutoService.Domain.Interfaces.Repositories;
using AutoService.Domain.Shared.Results;
using Microsoft.Extensions.Logging;

namespace AutoService.Application.Services;

/// <summary>
/// Operations for managing clients
/// </summary>
public class ClientService(
    IClientRepository clientRepository,
    IRepairOrderRepository repairOrderRepository,
    ILogger<ClientService> logger) : IClientService
{
    /// <inheritdoc />
    public async Task<List<ClientDto>> GetAllAsync()
    {
        logger.LogInformation("Getting all clients");

        var clients = await clientRepository.GetAllAsync();

        return clients.Select(ToDto).ToList();
    }

    /// <inheritdoc />
    public async Task<ClientDto?> GetByIdAsync(int id)
    {
        logger.LogInformation("Getting client with ID {ClientId}", id);

        var client = await clientRepository.GetByIdAsync(id);

        return client is null ? null : ToDto(client);
    }

    /// <inheritdoc />
    public async Task<List<RepeatedClientDto>> GetRepeatedLastMonthAsync()
    {
        logger.LogInformation("Getting clients with repeated repair orders during the last month");

        var monthAgo = DateTime.Now.AddMonths(-1);

        var orders = await repairOrderRepository.GetAllAsync();

        return orders
            .Where(order => order.AdmissionDate >= monthAgo)
            .GroupBy(order => order.ClientId)
            .Where(group => group.Count() > 1)
            .Select(group => new RepeatedClientDto
            {
                ClientId = group.Key,
                RequestsCount = group.Count()
            })
            .ToList();
    }

    /// <inheritdoc />
    public async Task<ClientDto> CreateAsync(CreateClientDto dto)
    {
        logger.LogInformation("Creating a new client");

        var client = new Client
        {
            FullName = dto.FullName,
            Phone = dto.Phone
        };

        client = await clientRepository.AddAsync(client);

        logger.LogInformation("Client with ID {ClientId} was created", client.Id);

        return ToDto(client);
    }

    /// <inheritdoc />
    public async Task<ClientDto?> UpdateAsync(int id, UpdateClientDto dto)
    {
        logger.LogInformation("Updating client with ID {ClientId}", id);

        var client = await clientRepository.GetByIdAsync(id);

        if (client is null)
        {
            logger.LogWarning("Client with ID {ClientId} was not found", id);

            return null;
        }

        client.FullName = dto.FullName;
        client.Phone = dto.Phone;

        var updated = await clientRepository.UpdateAsync(client);

        if (!updated)
        {
            logger.LogWarning("Client with ID {ClientId} could not be updated", id);

            return null;
        }

        logger.LogInformation("Client with ID {ClientId} was updated", id);

        return ToDto(client);
    }

    /// <inheritdoc />
    public async Task<DeleteResult> DeleteAsync(int id)
    {
        logger.LogInformation("Deleting client with ID {ClientId}", id);

        var client = await clientRepository.GetByIdAsync(id);

        if (client is null)
        {
            logger.LogWarning("Client with ID {ClientId} was not found", id);

            return DeleteResult.NotFound;
        }

        var hasCars = await clientRepository.HasCarsAsync(id);
        var hasOrders = await clientRepository.HasOrdersAsync(id);

        if (hasCars || hasOrders)
        {
            logger.LogWarning("Cannot delete client with ID {ClientId} because it has related cars or repair orders", id);

            return DeleteResult.HasRelatedEntities;
        }

        var deleted = await clientRepository.DeleteAsync(client);

        if (!deleted)
        {
            logger.LogWarning("Client with ID {ClientId} could not be deleted", id);

            return DeleteResult.NotFound;
        }

        logger.LogInformation("Client with ID {ClientId} was deleted", id);

        return DeleteResult.Deleted;
    }

    private static ClientDto ToDto(Client client)
    {
        return new ClientDto
        {
            Id = client.Id,
            FullName = client.FullName,
            Phone = client.Phone
        };
    }
}