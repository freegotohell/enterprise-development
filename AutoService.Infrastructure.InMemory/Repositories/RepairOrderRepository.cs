using AutoService.Domain.Data;
using AutoService.Domain.Entities;
using AutoService.Domain.Interfaces.Repositories;

namespace AutoService.Infrastructure.InMemory.Repositories;

/// <summary>
/// Repository for repair orders data operations
/// </summary>
public class RepairOrderRepository(AutoServiceContext context) : IRepairOrderRepository
{
    /// <inheritdoc />
    public Task<List<RepairOrder>> GetAllAsync()
    {
        return Task.FromResult(context.RepairOrders.ToList());
    }

    /// <inheritdoc />
    public Task<RepairOrder?> GetByIdAsync(int id)
    {
        var order = context.RepairOrders.FirstOrDefault(x => x.Id == id);

        return Task.FromResult(order);
    }

    /// <inheritdoc />
    public Task<RepairOrder> AddAsync(RepairOrder entity)
    {
        if (context.RepairOrders.Contains(entity))
        {
            return Task.FromResult(entity);
        }

        entity.Id = context.RepairOrders.Count == 0 ? 1 : context.RepairOrders.Max(order => order.Id) + 1;
        context.RepairOrders.Add(entity);

        return Task.FromResult(entity);
    }

    /// <inheritdoc />
    public Task<bool> UpdateAsync(RepairOrder entity)
    {
        var index = context.RepairOrders.FindIndex(order => order.Id == entity.Id);

        if (index == -1)
        {
            return Task.FromResult(false);
        }

        context.RepairOrders[index] = entity;

        return Task.FromResult(true);
    }

    /// <inheritdoc />
    public Task<bool> DeleteAsync(RepairOrder entity)
    {
        var existing = context.RepairOrders.FirstOrDefault(order => order.Id == entity.Id);

        if (existing is null)
        {
            return Task.FromResult(false);
        }

        context.RepairOrders.Remove(existing);

        return Task.FromResult(true);
    }

    /// <inheritdoc />
    public Task<List<RepairOrder>> GetByClientIdAsync(int clientId)
    {
        var orders = context.RepairOrders.Where(order => order.ClientId == clientId).ToList();

        return Task.FromResult(orders);
    }

    /// <inheritdoc />
    public Task<List<RepairOrder>> GetByCarIdAsync(int carId)
    {
        var orders = context.RepairOrders.Where(order => order.CarId == carId).ToList();

        return Task.FromResult(orders);
    }

    /// <inheritdoc />
    public Task<List<RepairOrder>> GetByMechanicIdAsync(int mechanicId)
    {
        var orders = context.RepairOrders.Where(order => order.Mechanics.Any(orderMechanic => orderMechanic.MechanicId == mechanicId)).ToList();

        return Task.FromResult(orders);
    }

    /// <inheritdoc />
    public Task<List<RepairOrder>> GetByWorkTypeIdAsync(int workTypeId)
    {
        var orders = context.RepairOrders.Where(order => order.Works.Any(orderWork => orderWork.WorkTypeId == workTypeId)).ToList();

        return Task.FromResult(orders);
    }
}