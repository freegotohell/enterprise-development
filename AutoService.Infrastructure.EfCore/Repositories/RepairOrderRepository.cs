using AutoService.Domain.Entities;
using AutoService.Domain.Interfaces.Repositories;
using AutoService.Infrastructure.EfCore.Data;
using Microsoft.EntityFrameworkCore;

namespace AutoService.Infrastructure.EfCore.Repositories;

/// <summary>
/// Repository for repair order data operations
/// </summary>
public class RepairOrderRepository(AutoServiceDbContext context) : IRepairOrderRepository
{
    private IQueryable<RepairOrder> GetOrdersWithDetails()
    {
        return context.RepairOrders
            .Include(order => order.Client)
            .Include(order => order.Car)
            .Include(order => order.Mechanics).ThenInclude(orderMechanic => orderMechanic.Mechanic)
            .Include(order => order.Works).ThenInclude(orderWork => orderWork.WorkType);
    }

    /// <inheritdoc />
    public async Task<List<RepairOrder>> GetAllAsync()
    {
        return await GetOrdersWithDetails().AsNoTracking().ToListAsync();
    }

    /// <inheritdoc />
    public async Task<RepairOrder?> GetByIdAsync(int id)
    {
        return await GetOrdersWithDetails().FirstOrDefaultAsync(order => order.Id == id);
    }

    /// <inheritdoc />
    public async Task<RepairOrder> AddAsync(RepairOrder entity)
    {
        await context.RepairOrders.AddAsync(entity);
        await context.SaveChangesAsync();

        return entity;
    }

    /// <inheritdoc />
    public async Task<bool> UpdateAsync(RepairOrder entity)
    {
        RepairOrder? existingOrder = await context.RepairOrders.FirstOrDefaultAsync(order => order.Id == entity.Id);

        if (existingOrder is null)
        {
            return false;
        }

        await context.Entry(existingOrder).Collection(order => order.Mechanics).LoadAsync();

        await context.Entry(existingOrder).Collection(order => order.Works).LoadAsync();

        existingOrder.ClientId = entity.ClientId;
        existingOrder.CarId = entity.CarId;
        existingOrder.AdmissionDate = entity.AdmissionDate;
        existingOrder.ReleaseDate = entity.ReleaseDate;

        var newMechanicIds = entity.Mechanics.Select(orderMechanic => orderMechanic.MechanicId).ToHashSet();
        var newWorkTypeIds = entity.Works.Select(orderWork => orderWork.WorkTypeId).ToHashSet();

        context.OrderMechanics.RemoveRange(existingOrder.Mechanics.Where(orderMechanic => !newMechanicIds.Contains(orderMechanic.MechanicId)));
        context.OrderWorks.RemoveRange(existingOrder.Works.Where(orderWork => !newWorkTypeIds.Contains(orderWork.WorkTypeId)));

        var existingMechanicIds = existingOrder.Mechanics.Select(orderMechanic => orderMechanic.MechanicId).ToHashSet();

        foreach (var mechanicId in newMechanicIds.Except(existingMechanicIds))
        {
            existingOrder.Mechanics.Add(new OrderMechanic
            {
                RepairOrderId = existingOrder.Id,
                MechanicId = mechanicId
            });
        }

        var existingWorkTypeIds = existingOrder.Works.Select(orderWork => orderWork.WorkTypeId).ToHashSet();

        foreach (var workTypeId in newWorkTypeIds.Except(existingWorkTypeIds))
        {
            existingOrder.Works.Add(new OrderWork
            {
                RepairOrderId = existingOrder.Id,
                WorkTypeId = workTypeId
            });
        }

        await context.SaveChangesAsync();

        return true;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(RepairOrder entity)
    {
        RepairOrder? existingOrder = await context.RepairOrders.FindAsync(entity.Id);

        if (existingOrder is null)
        {
            return false;
        }

        context.RepairOrders.Remove(existingOrder);
        await context.SaveChangesAsync();

        return true;
    }

    /// <inheritdoc />
    public async Task<List<RepairOrder>> GetByClientIdAsync(int clientId)
    {
        return await GetOrdersWithDetails().AsNoTracking().Where(order => order.ClientId == clientId).ToListAsync();
    }

    /// <inheritdoc />
    public async Task<List<RepairOrder>> GetByCarIdAsync(int carId)
    {
        return await GetOrdersWithDetails().AsNoTracking().Where(order => order.CarId == carId).ToListAsync();
    }

    /// <inheritdoc />
    public async Task<List<RepairOrder>> GetByMechanicIdAsync(int mechanicId)
    {
        return await GetOrdersWithDetails()
            .AsNoTracking()
            .Where(order => order.Mechanics.Any(orderMechanic => orderMechanic.MechanicId == mechanicId))
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<List<RepairOrder>> GetByWorkTypeIdAsync(int workTypeId)
    {
        return await GetOrdersWithDetails()
            .AsNoTracking()
            .Where(order => order.Works.Any(orderWork => orderWork.WorkTypeId == workTypeId))
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<List<RepairOrder>> GetByAdmissionDateFromAsync(DateTime date)
    {
        return await GetOrdersWithDetails()
            .AsNoTracking()
            .Where(order => order.AdmissionDate >= date)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<List<(WorkType WorkType, int Count)>> GetTop5MostFrequentWorkTypesAsync()
    {
        var result = await context.OrderWorks
            .AsNoTracking()
            .GroupBy(orderWork => orderWork.WorkTypeId)
            .Select(group => new
            {
                WorkTypeId = group.Key,
                Count = group.Count()
            })
            .Join(
                context.WorkTypes.AsNoTracking(),
                group => group.WorkTypeId,
                workType => workType.Id,
                (group, workType) => new
                {
                    WorkType = workType,
                    group.Count
                })
            .OrderByDescending(item => item.Count)
            .ThenBy(item => item.WorkType.Id)
            .Take(5)
            .ToListAsync();

        return [.. result.Select(item => (item.WorkType, item.Count))];
    }
}
