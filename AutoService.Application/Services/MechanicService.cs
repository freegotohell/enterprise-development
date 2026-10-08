using AutoService.Contracts.DTOs;
using AutoService.Contracts.Services;
using AutoService.Domain.Entities;
using AutoService.Domain.Interfaces.Repositories;
using AutoService.Domain.Shared.Enums;
using AutoService.Domain.Shared.Mapping;
using AutoService.Domain.Shared.Results;
using Microsoft.Extensions.Logging;

namespace AutoService.Application.Services;

/// <summary>
/// Operations for managing mechanics
/// </summary>
public class MechanicService(
    IMechanicRepository mechanicRepository,
    IRepairOrderRepository repairOrderRepository,
    IWorkTypeRepository workTypeRepository,
    ILogger<MechanicService> logger) : IMechanicService
{
    /// <inheritdoc />
    public async Task<List<MechanicDto>> GetAllAsync()
    {
        logger.LogInformation("Getting all mechanics");

        List<Mechanic> mechanics = await mechanicRepository.GetAllAsync();

        return mechanics.Select(ToDto).ToList();
    }

    /// <inheritdoc />
    public async Task<MechanicDto?> GetByIdAsync(int id)
    {
        logger.LogInformation("Getting mechanic with ID {MechanicId}", id);

        Mechanic? mechanic = await mechanicRepository.GetByIdAsync(id);

        return mechanic is null ? null : ToDto(mechanic);
    }

    /// <inheritdoc />
    public async Task<List<ClientDto>?> GetClientsAsync(int mechanicId)
    {
        logger.LogInformation("Getting clients for mechanic with ID {MechanicId}", mechanicId);

        Mechanic? mechanic = await mechanicRepository.GetByIdAsync(mechanicId);

        if (mechanic is null)
        {
            logger.LogWarning("Mechanic with ID {MechanicId} was not found", mechanicId);

            return null;
        }

        List<RepairOrder> orders = await repairOrderRepository.GetByMechanicIdAsync(mechanicId);

        return orders
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

    /// <inheritdoc />
    public async Task<List<MechanicDto>?> GetByWorkTypeIdAsync(int workTypeId)
    {
        logger.LogInformation("Getting mechanics for work type with ID {WorkTypeId}", workTypeId);

        WorkType? workType = await workTypeRepository.GetByIdAsync(workTypeId);

        if (workType is null)
        {
            logger.LogWarning("Work type with ID {WorkTypeId} was not found", workTypeId);

            return null;
        }

        List<Mechanic> mechanics = [];

        foreach (MechanicSpecialization specialization in Enum.GetValues<MechanicSpecialization>())
        {
            if (MechanicSpecializationMapping.Matches(specialization, workType.Category))
            {
                List<Mechanic> specializationMechanics = await mechanicRepository.GetBySpecializationAsync(specialization);

                mechanics.AddRange(specializationMechanics);
            }
        }

        return mechanics.OrderBy(mechanic => mechanic.FullName).Select(ToDto).ToList();
    }

    /// <inheritdoc />
    public async Task<List<MechanicDto>> GetBySpecializationAsync(MechanicSpecialization specialization)
    {
        logger.LogInformation("Getting mechanics with specialization {Specialization}", specialization);

        List<Mechanic> mechanics = await mechanicRepository.GetBySpecializationAsync(specialization);

        return mechanics.Select(ToDto).ToList();
    }

    /// <inheritdoc />
    public async Task<MechanicDto> CreateAsync(CreateMechanicDto dto)
    {
        logger.LogInformation("Creating a new mechanic");

        List<Mechanic> mechanics = await mechanicRepository.GetAllAsync();

        Mechanic mechanic = new()
        {
            Id = mechanics.Count == 0 ? 1 : mechanics.Max(x => x.Id) + 1,
            PassportNumber = dto.PassportNumber,
            FullName = dto.FullName,
            Specialization = dto.Specialization,
            Experience = dto.Experience
        };

        await mechanicRepository.AddAsync(mechanic);

        logger.LogInformation("Mechanic with ID {MechanicId} was created", mechanic.Id);

        return ToDto(mechanic);
    }

    /// <inheritdoc />
    public async Task<MechanicDto?> UpdateAsync(int id, UpdateMechanicDto dto)
    {
        logger.LogInformation("Updating mechanic with ID {MechanicId}", id);

        Mechanic? mechanic = await mechanicRepository.GetByIdAsync(id);

        if (mechanic is null)
        {
            logger.LogWarning("Mechanic with ID {MechanicId} was not found", id);

            return null;
        }

        mechanic.PassportNumber = dto.PassportNumber;
        mechanic.FullName = dto.FullName;
        mechanic.Specialization = dto.Specialization;
        mechanic.Experience = dto.Experience;

        await mechanicRepository.UpdateAsync(mechanic);

        logger.LogInformation("Mechanic with ID {MechanicId} was updated", id);

        return ToDto(mechanic);
    }

    /// <inheritdoc />
    public async Task<DeleteResult> DeleteAsync(int id)
    {
        logger.LogInformation("Deleting mechanic with ID {MechanicId}", id);

        Mechanic? mechanic = await mechanicRepository.GetByIdAsync(id);

        if (mechanic is null)
        {
            logger.LogWarning("Mechanic with ID {MechanicId} was not found", id);

            return DeleteResult.NotFound;
        }

        var hasOrders = await mechanicRepository.HasOrdersAsync(id);

        if (hasOrders)
        {
            logger.LogWarning("Cannot delete mechanic with ID {MechanicId} because it has repair orders", id);

            return DeleteResult.HasRelatedEntities;
        }

        await mechanicRepository.DeleteAsync(mechanic);

        logger.LogInformation("Mechanic with ID {MechanicId} was deleted", id);

        return DeleteResult.Deleted;
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