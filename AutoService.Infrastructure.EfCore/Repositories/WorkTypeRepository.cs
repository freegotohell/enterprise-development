using AutoService.Domain.Entities;
using AutoService.Domain.Interfaces.Repositories;
using AutoService.Infrastructure.EfCore.Data;
using Microsoft.EntityFrameworkCore;

namespace AutoService.Infrastructure.EfCore.Repositories;

/// <summary>
/// Repository for work type data operations
/// </summary>
public class WorkTypeRepository(AutoServiceDbContext context) : IWorkTypeRepository
{
    /// <inheritdoc />
    public async Task<List<WorkType>> GetAllAsync()
    {
        return await context.WorkTypes.AsNoTracking().ToListAsync();
    }

    /// <inheritdoc />
    public async Task<WorkType?> GetByIdAsync(int id)
    {
        return await context.WorkTypes.FirstOrDefaultAsync(workType => workType.Id == id);
    }

    /// <inheritdoc />
    public async Task<WorkType> AddAsync(WorkType entity)
    {
        await context.WorkTypes.AddAsync(entity);
        await context.SaveChangesAsync();

        return entity;
    }

    /// <inheritdoc />
    public async Task<bool> UpdateAsync(WorkType entity)
    {
        context.WorkTypes.Update(entity);

        try
        {
            await context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            return false;
        }
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(WorkType entity)
    {
        context.WorkTypes.Remove(entity);

        try
        {
            await context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            return false;
        }
    }

    /// <inheritdoc />
    public Task<bool> HasOrdersAsync(int workTypeId)
    {
        return context.OrderWorks.AnyAsync(orderWork => orderWork.WorkTypeId == workTypeId);
    }

    /// <inheritdoc />
    public Task<List<WorkType>> GetByIdsAsync(IReadOnlyCollection<int> ids)
    {
        return context.WorkTypes.Where(workType => ids.Contains(workType.Id)).ToListAsync();
    }
}
