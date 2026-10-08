using AutoService.Domain.Data;
using AutoService.Domain.Entities;
using AutoService.Domain.Interfaces.Repositories;

namespace AutoService.Infrastructure.InMemory.Repositories;

/// <summary>
/// Repository for repair orders data operations
/// </summary>
public class RepairOrderRepository : IRepairOrderRepository
{
    private readonly AutoServiceContext _context;

    /// <summary>
    /// Initializes a new instance of the repo
    /// </summary>
    public RepairOrderRepository(AutoServiceContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public Task<List<RepairOrder>> GetAllAsync()
    {
        return Task.FromResult(_context.RepairOrders.ToList());
    }

    /// <inheritdoc />
    public Task<RepairOrder?> GetByIdAsync(int id)
    {
        var order = _context.RepairOrders.FirstOrDefault(x => x.Id == id);

        return Task.FromResult(order);
    }

    /// <inheritdoc />
    public Task AddAsync(RepairOrder entity)
    {
        _context.RepairOrders.Add(entity);

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task UpdateAsync(RepairOrder entity)
    {
        var existing = _context.RepairOrders.FirstOrDefault(x => x.Id == entity.Id);

        if (existing is not null)
        {
            var index = _context.RepairOrders.IndexOf(existing);
            _context.RepairOrders[index] = entity;
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task DeleteAsync(RepairOrder entity)
    {
        _context.RepairOrders.Remove(entity);

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task<List<RepairOrder>> GetByClientIdAsync(int clientId)
    {
        var orders = _context.RepairOrders.Where(order => order.ClientId == clientId).ToList();

        return Task.FromResult(orders);
    }

    /// <inheritdoc />
    public Task<List<RepairOrder>> GetByCarIdAsync(int carId)
    {
        var orders = _context.RepairOrders.Where(order => order.CarId == carId).ToList();

        return Task.FromResult(orders);
    }

    /// <inheritdoc />
    public Task<List<RepairOrder>> GetByMechanicIdAsync(int mechanicId)
    {
        var orders = _context.RepairOrders.Where(order => order.Mechanics.Any(orderMechanic => orderMechanic.MechanicId == mechanicId)).ToList();

        return Task.FromResult(orders);
    }

    /// <inheritdoc />
    public Task<List<RepairOrder>> GetByWorkTypeIdAsync(int workTypeId)
    {
        var orders = _context.RepairOrders.Where(order => order.Works.Any(orderWork => orderWork.WorkTypeId == workTypeId)).ToList();

        return Task.FromResult(orders);
    }
}