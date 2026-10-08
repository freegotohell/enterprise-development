using AutoService.Domain.Data;
using AutoService.Domain.Entities;
using AutoService.Domain.Interfaces.Repositories;

namespace AutoService.Infrastructure.InMemory.Repositories;

/// <summary>
/// Repository for work types data operations
/// </summary>
public class WorkTypeRepository : IWorkTypeRepository
{
    private readonly AutoServiceContext _context;

    /// <summary>
    /// Initializes a new instance of the repo
    /// </summary>
    public WorkTypeRepository(AutoServiceContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public Task<List<WorkType>> GetAllAsync()
    {
        return Task.FromResult(_context.WorkTypes.ToList());
    }

    /// <inheritdoc />
    public Task<WorkType?> GetByIdAsync(int id)
    {
        var workType = _context.WorkTypes.FirstOrDefault(x => x.Id == id);

        return Task.FromResult(workType);
    }

    /// <inheritdoc />
    public Task AddAsync(WorkType entity)
    {
        _context.WorkTypes.Add(entity);

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task UpdateAsync(WorkType entity)
    {
        var existing = _context.WorkTypes.FirstOrDefault(x => x.Id == entity.Id);

        if (existing is not null)
        {
            var index = _context.WorkTypes.IndexOf(existing);
            _context.WorkTypes[index] = entity;
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task DeleteAsync(WorkType entity)
    {
        _context.WorkTypes.Remove(entity);

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task<bool> HasOrdersAsync(int workTypeId)
    {
        var hasOrders = _context.RepairOrders.Any(order => order.Works.Any(orderWork => orderWork.WorkTypeId == workTypeId));

        return Task.FromResult(hasOrders);
    }
}