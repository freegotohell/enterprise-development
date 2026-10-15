using AutoService.Domain.Data;
using AutoService.Domain.Entities;
using AutoService.Domain.Interfaces.Repositories;

namespace AutoService.Infrastructure.InMemory.Repositories;

/// <summary>
/// Repository for work types data operations
/// </summary>
public class WorkTypeRepository(AutoServiceContext context) : IWorkTypeRepository
{
    /// <inheritdoc />
    public Task<List<WorkType>> GetAllAsync()
    {
        return Task.FromResult(context.WorkTypes.ToList());
    }

    /// <inheritdoc />
    public Task<WorkType?> GetByIdAsync(int id)
    {
        var workType = context.WorkTypes.FirstOrDefault(x => x.Id == id);

        return Task.FromResult(workType);
    }

    /// <inheritdoc />
    public Task<WorkType> AddAsync(WorkType entity)
    {
        if (context.WorkTypes.Contains(entity))
        {
            return Task.FromResult(entity);
        }

        entity.Id = context.WorkTypes.Count == 0 ? 1 : context.WorkTypes.Max(workType => workType.Id) + 1;
        context.WorkTypes.Add(entity);

        return Task.FromResult(entity);
    }

    /// <inheritdoc />
    public Task<bool> UpdateAsync(WorkType entity)
    {
        var index = context.WorkTypes.FindIndex(workType => workType.Id == entity.Id);

        if (index == -1)
        {
            return Task.FromResult(false);
        }

        context.WorkTypes[index] = entity;

        return Task.FromResult(true);
    }

    /// <inheritdoc />
    public Task<bool> DeleteAsync(WorkType entity)
    {
        var existing = context.WorkTypes.FirstOrDefault(workType => workType.Id == entity.Id);

        if (existing is null)
        {
            return Task.FromResult(false);
        }

        context.WorkTypes.Remove(existing);

        return Task.FromResult(true);
    }

    /// <inheritdoc />
    public Task<bool> HasOrdersAsync(int workTypeId)
    {
        var hasOrders = context.RepairOrders.Any(order => order.Works.Any(orderWork => orderWork.WorkTypeId == workTypeId));

        return Task.FromResult(hasOrders);
    }
}