using AutoService.Contracts.DTOs;
using AutoService.Domain.Data;
using AutoService.Domain.Entities;
using AutoService.Domain.Shared.Results;

namespace AutoService.Api.Services;

/// <summary>
/// Operations for managing clients
/// </summary>
public class ClientService(AutoServiceContext context, ILogger<ClientService> logger)
{
    /// <summary>
    /// Gets all clients
    /// </summary>
    public List<ClientDto> GetAll()
    {
        logger.LogInformation("Getting all clients");

        return context.Clients.Select(ToDto).ToList();
    }

    /// <summary>
    /// Gets a client by id
    /// </summary>
    public ClientDto? GetById(int id)
    {
        logger.LogInformation("Getting client with ID {ClientId}", id);

        Client? client = context.Clients.FirstOrDefault(x => x.Id == id);

        return client is null ? null : ToDto(client);
    }

    /// <summary>
    /// Gets clients with more than one repair order during the last month
    /// </summary>
    public List<RepeatedClientDto> GetRepeatedLastMonth()
    {
        logger.LogInformation("Getting clients with repeated repair orders during the last month");

        DateTime monthAgo = DateTime.Now.AddMonths(-1);

        return context.RepairOrders
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

    /// <summary>
    /// Creates a new client
    /// </summary>
    public ClientDto Create(CreateClientDto dto)
    {
        logger.LogInformation("Creating a new client");

        var client = new Client
        {
            Id = context.Clients.Count == 0 ? 1 : context.Clients.Max(x => x.Id) + 1,
            FullName = dto.FullName,
            Phone = dto.Phone
        };

        context.Clients.Add(client);

        logger.LogInformation("Client with ID {ClientId} was created", client.Id);

        return ToDto(client);
    }

    /// <summary>
    /// Updates an existing client
    /// </summary>
    public ClientDto? Update(int id, UpdateClientDto dto)
    {
        logger.LogInformation("Updating client with ID {ClientId}", id);

        Client? client = context.Clients.FirstOrDefault(x => x.Id == id);

        if (client is null)
        {
            logger.LogWarning("Client with ID {ClientId} was not found", id);

            return null;
        }

        client.FullName = dto.FullName;
        client.Phone = dto.Phone;

        logger.LogInformation("Client with ID {ClientId} was updated", id);

        return ToDto(client);
    }

    /// <summary>
    /// Deletes a client by id
    /// </summary>
    public DeleteResult Delete(int id)
    {
        logger.LogInformation("Deleting client with ID {ClientId}", id);

        Client? client = context.Clients.FirstOrDefault(x => x.Id == id);

        if (client is null)
        {
            logger.LogWarning("Client with ID {ClientId} was not found", id);

            return DeleteResult.NotFound;
        }

        if (client.Cars.Count > 0 || client.Orders.Count > 0)
        {
            logger.LogWarning("Cannot delete client with ID {ClientId} because it has related cars or repair orders", id);

            return DeleteResult.HasRelatedEntities;
        }

        context.Clients.Remove(client);

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