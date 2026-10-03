using AutoService.Contracts.DTOs;
using AutoService.Domain.Data;
using AutoService.Domain.Entities;

namespace AutoService.Api.Services;

/// <summary>
/// Operations for managing mechanics
/// </summary>
public class MechanicService(
    AutoServiceContext context,
    ILogger<MechanicService> logger)
{
    /// <summary>
    /// Gets all mechanics
    /// </summary>
    public List<MechanicDto> GetAll()
    {
        logger.LogInformation("Getting all mechanics");

        return context.Mechanics.Select(ToDto).ToList();
    }

    /// <summary>
    /// Gets a mechanic by id
    /// </summary>
    public MechanicDto? GetById(int id)
    {
        logger.LogInformation("Getting mechanic with ID {MechanicId}", id);

        var mechanic = context.Mechanics.FirstOrDefault(x => x.Id == id);

        return mechanic is null ? null : ToDto(mechanic);
    }

    /// <summary>
    /// Creates a new mechanic
    /// </summary>
    public MechanicDto Create(CreateMechanicDto dto)
    {
        logger.LogInformation("Creating a new mechanic");

        var mechanic = new Mechanic
        {
            Id = context.Mechanics.Count == 0 ? 1 : context.Mechanics.Max(x => x.Id) + 1,
            PassportNumber = dto.PassportNumber,
            FullName = dto.FullName,
            Specialization = dto.Specialization,
            Experience = dto.Experience
        };

        context.Mechanics.Add(mechanic);

        logger.LogInformation("Mechanic with ID {MechanicId} was created", mechanic.Id);

        return ToDto(mechanic);
    }

    /// <summary>
    /// Updates an existing mechanic
    /// </summary>
    public MechanicDto? Update(int id, UpdateMechanicDto dto)
    {
        logger.LogInformation("Updating mechanic with ID {MechanicId}", id);

        var mechanic = context.Mechanics.FirstOrDefault(x => x.Id == id);

        if (mechanic is null)
        {
            logger.LogWarning("Mechanic with ID {MechanicId} was not found", id);

            return null;
        }

        mechanic.PassportNumber = dto.PassportNumber;
        mechanic.FullName = dto.FullName;
        mechanic.Specialization = dto.Specialization;
        mechanic.Experience = dto.Experience;

        logger.LogInformation("Mechanic with ID {MechanicId} was updated", id);

        return ToDto(mechanic);
    }

    /// <summary>
    /// Deletes a mechanic by id
    /// </summary>
    public bool Delete(int id)
    {
        logger.LogInformation("Deleting mechanic with ID {MechanicId}", id);

        var mechanic = context.Mechanics.FirstOrDefault(x => x.Id == id);

        if (mechanic is null)
        {
            logger.LogWarning("Mechanic with ID {MechanicId} was not found", id);

            return false;
        }

        context.Mechanics.Remove(mechanic);

        logger.LogInformation("Mechanic with ID {MechanicId} was deleted", id);

        return true;
    }

    /// <summary>
    /// Retrieves list of clients whose repair orders associated with a specific mechanic
    /// </summary>
    public List<ClientDto>? GetClients(int mechanicId)
    {
        logger.LogInformation("Getting clients for mechanic with ID {MechanicId}", mechanicId);

        var mechanicExists = context.Mechanics.Any(x => x.Id == mechanicId);

        if (!mechanicExists)
        {
            logger.LogWarning( "Mechanic with ID {MechanicId} was not found", mechanicId);

            return null;
        }

        return context.RepairOrders
            .Where(order => order.Mechanics.Any(orderMechanic => orderMechanic.MechanicId == mechanicId))
            .Select(order => order.Client)
            .Distinct()
            .OrderBy(client => client.FullName)
            .Select(client => new ClientDto
            {
                Id = client.Id,
                FullName = client.FullName,
                Phone = client.Phone
            })
            .ToList();
    }
    private static MechanicDto ToDto(Mechanic mechanic)
    {
        return new MechanicDto
        {
            Id = mechanic.Id,
            PassportNumber = mechanic.PassportNumber,
            FullName = mechanic.FullName,
            Specialization = mechanic.Specialization,
            Experience = mechanic.Experience
        };
    }
}