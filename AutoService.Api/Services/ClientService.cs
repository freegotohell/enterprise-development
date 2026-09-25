using AutoService.Contracts.DTOs;
using AutoService.Domain.Data;
using AutoService.Domain.Entities;

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
        logger.LogInformation("Getting all clients.");

        return context.Clients.Select(ToDto).ToList();
    }

    /// <summary>
    /// Gets a client by id
    /// </summary>
    public ClientDto? GetById(int id)
    {
        logger.LogInformation("Getting client with ID {ClientId}.", id);

        var client = context.Clients.FirstOrDefault(x => x.Id == id);

        return client is null ? null : ToDto(client);
    }

    /// <summary>
    /// Creates a new client
    /// </summary>
    public ClientDto Create(CreateClientDto dto)
    {
        logger.LogInformation("Creating a new client.");

        var client = new Client
        {
            Id = context.Clients.Count == 0 ? 1 : context.Clients.Max(x => x.Id) + 1,
            FullName = dto.FullName,
            Phone = dto.Phone
        };

        context.Clients.Add(client);

        logger.LogInformation("Client with ID {ClientId} was created.", client.Id);

        return ToDto(client);
    }

    /// <summary>
    /// Updates an existing client
    /// </summary>
    public ClientDto? Update(int id, UpdateClientDto dto)
    {
        logger.LogInformation("Updating client with ID {ClientId}.", id);

        var client = context.Clients.FirstOrDefault(x => x.Id == id);

        if (client is null)
        {
            logger.LogWarning("Client with ID {ClientId} was not found.", id);

            return null;
        }

        client.FullName = dto.FullName;
        client.Phone = dto.Phone;

        logger.LogInformation("Client with ID {ClientId} was updated.", id);

        return ToDto(client);
    }

    /// <summary>
    /// Deletes a client by identifier
    /// </summary>
    public bool Delete(int id)
    {
        logger.LogInformation("Deleting client with ID {ClientId}.", id);

        var client = context.Clients.FirstOrDefault(x => x.Id == id);

        if (client is null)
        {
            logger.LogWarning("Client with ID {ClientId} was not found.", id);

            return false;
        }

        context.Clients.Remove(client);

        logger.LogInformation("Client with ID {ClientId} was deleted.", id);

        return true;
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