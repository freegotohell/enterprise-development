using AutoService.Contracts.DTOs;
using AutoService.Domain.Data;
using AutoService.Domain.Entities;
using AutoService.Domain.Shared.Results;

namespace AutoService.Api.Services;

/// <summary>
/// Operations for managing work types
/// </summary>
public class WorkTypeService(AutoServiceContext context, ILogger<WorkTypeService> logger)
{
    /// <summary>
    /// Gets all work types
    /// </summary>
    public List<WorkTypeDto> GetAll()
    {
        logger.LogInformation("Getting all work types");

        return context.WorkTypes.Select(ToDto).ToList();
    }

    /// <summary>
    /// Gets a work type by id
    /// </summary>
    public WorkTypeDto? GetById(int id)
    {
        logger.LogInformation("Getting work type with ID {WorkTypeId}", id);

        WorkType? workType = context.WorkTypes.FirstOrDefault(x => x.Id == id);

        return workType is null ? null : ToDto(workType);
    }

    /// <summary>
    /// Gets five most frequently performed work types
    /// </summary>
    public List<FrequentWorkTypeDto> GetTop5MostFrequent()
    {
        logger.LogInformation("Getting top 5 most frequently performed work types");

        return context.RepairOrders
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

    /// <summary>
    /// Creates a new work type
    /// </summary>
    public WorkTypeDto Create(CreateWorkTypeDto dto)
    {
        logger.LogInformation("Creating a new work type");

        var workType = new WorkType
        {
            Id = context.WorkTypes.Count == 0 ? 1 : context.WorkTypes.Max(x => x.Id) + 1,
            Name = dto.Name,
            Category = dto.Category,
            Cost = dto.Cost,
            Duration = dto.Duration,
            Description = dto.Description
        };

        context.WorkTypes.Add(workType);

        logger.LogInformation("Work type with ID {WorkTypeId} was created", workType.Id);

        return ToDto(workType);
    }

    /// <summary>
    /// Updates an existing work type
    /// </summary>
    public WorkTypeDto? Update(int id, UpdateWorkTypeDto dto)
    {
        logger.LogInformation("Updating work type with ID {WorkTypeId}", id);

        WorkType? workType = context.WorkTypes.FirstOrDefault(x => x.Id == id);

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

        logger.LogInformation("Work type with ID {WorkTypeId} was updated", id);

        return ToDto(workType);
    }

    /// <summary>
    /// Deletes a work type by id
    /// </summary>
    public DeleteResult Delete(int id)
    {
        logger.LogInformation("Deleting work type with ID {WorkTypeId}", id);

        WorkType? workType = context.WorkTypes.FirstOrDefault(x => x.Id == id);

        if (workType is null)
        {
            logger.LogWarning("Work type with ID {WorkTypeId} was not found", id);

            return DeleteResult.NotFound;
        }

        if (workType.Orders.Count > 0)
        {
            logger.LogWarning("Cannot delete work type with ID {WorkTypeId} because it has repair orders", id);

            return DeleteResult.HasRelatedEntities;
        }

        context.WorkTypes.Remove(workType);

        logger.LogInformation("Work type with ID {WorkTypeId} was deleted", id);

        return DeleteResult.Deleted;
    }

    /// <summary>
    /// Fills WorkTypeDto with WorkType entity
    /// </summary>
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
