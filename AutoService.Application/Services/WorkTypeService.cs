using AutoService.Contracts.DTOs;
using AutoService.Contracts.Services;
using AutoService.Domain.Entities;
using AutoService.Domain.Interfaces.Repositories;
using AutoService.Domain.Shared.Results;
using Microsoft.Extensions.Logging;

namespace AutoService.Application.Services;

/// <summary>
/// Operations for managing work types
/// </summary>
public class WorkTypeService(
    IWorkTypeRepository workTypeRepository,
    IRepairOrderRepository repairOrderRepository,
    ILogger<WorkTypeService> logger) : IWorkTypeService
{
    /// <inheritdoc />
    public async Task<List<WorkTypeDto>> GetAllAsync()
    {
        logger.LogInformation("Getting all work types");

        List<WorkType> workTypes = await workTypeRepository.GetAllAsync();

        return workTypes.Select(ToDto).ToList();
    }

    /// <inheritdoc />
    public async Task<WorkTypeDto?> GetByIdAsync(int id)
    {
        logger.LogInformation("Getting work type with ID {WorkTypeId}", id);

        WorkType? workType = await workTypeRepository.GetByIdAsync(id);

        return workType is null ? null : ToDto(workType);
    }

    /// <inheritdoc />
    public async Task<List<FrequentWorkTypeDto>> GetTop5MostFrequentAsync()
    {
        logger.LogInformation("Getting top 5 most frequently performed work types");

        List<RepairOrder> repairOrders = await repairOrderRepository.GetAllAsync();

        return repairOrders
            .SelectMany(order => order.Works)
            .GroupBy(orderWork => orderWork.WorkType)
            .Select(group => new FrequentWorkTypeDto
            {
                WorkTypeId = group.Key.Id,
                Name = group.Key.Name,
                Count = group.Count()
            })
            .OrderByDescending(x => x.Count)
            .Take(5)
            .ToList();
    }

    /// <inheritdoc />
    public async Task<WorkTypeDto> CreateAsync(CreateWorkTypeDto dto)
    {
        logger.LogInformation("Creating a new work type");

        List<WorkType> workTypes = await workTypeRepository.GetAllAsync();

        var workType = new WorkType
        {
            Id = workTypes.Count == 0 ? 1 : workTypes.Max(x => x.Id) + 1,
            Name = dto.Name,
            Category = dto.Category,
            Cost = dto.Cost,
            Duration = dto.Duration,
            Description = dto.Description
        };

        await workTypeRepository.AddAsync(workType);

        logger.LogInformation("Work type with ID {WorkTypeId} was created", workType.Id);

        return ToDto(workType);
    }

    /// <inheritdoc />
    public async Task<WorkTypeDto?> UpdateAsync(int id, UpdateWorkTypeDto dto)
    {
        logger.LogInformation("Updating work type with ID {WorkTypeId}", id);

        WorkType? workType = await workTypeRepository.GetByIdAsync(id);

        if (workType is null)
        {
            logger.LogWarning("Work type with ID {WorkTypeId} was not found", id);

            return null;
        }

        workType.Name = dto.Name;
        workType.Category = dto.Category;
        workType.Cost = dto.Cost;
        workType.Duration = dto.Duration;
        workType.Description = dto.Description;

        await workTypeRepository.UpdateAsync(workType);

        logger.LogInformation("Work type with ID {WorkTypeId} was updated", id);

        return ToDto(workType);
    }

    /// <inheritdoc />
    public async Task<DeleteResult> DeleteAsync(int id)
    {
        logger.LogInformation("Deleting work type with ID {WorkTypeId}", id);

        WorkType? workType = await workTypeRepository.GetByIdAsync(id);

        if (workType is null)
        {
            logger.LogWarning("Work type with ID {WorkTypeId} was not found", id);

            return DeleteResult.NotFound;
        }

        var hasOrders = await workTypeRepository.HasOrdersAsync(id);

        if (hasOrders)
        {
            logger.LogWarning("Cannot delete work type with ID {WorkTypeId} because it has repair orders", id);

            return DeleteResult.HasRelatedEntities;
        }

        await workTypeRepository.DeleteAsync(workType);

        logger.LogInformation("Work type with ID {WorkTypeId} was deleted", id);

        return DeleteResult.Deleted;
    }

    private static WorkTypeDto ToDto(WorkType workType)
    {
        return new WorkTypeDto
        {
            Id = workType.Id,
            Name = workType.Name,
            Category = workType.Category,
            Cost = workType.Cost,
            Duration = workType.Duration,
            Description = workType.Description
        };
    }
}