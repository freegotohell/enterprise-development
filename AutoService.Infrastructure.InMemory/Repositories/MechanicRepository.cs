using AutoService.Domain.Data;
using AutoService.Domain.Entities;
using AutoService.Domain.Interfaces.Repositories;
using AutoService.Domain.Shared.Enums;

namespace AutoService.Infrastructure.InMemory.Repositories;

/// <summary>
/// Repository for mechanics data operations
/// </summary>
public class MechanicRepository : IMechanicRepository
{
    private readonly AutoServiceContext _context;

    /// <summary>
    /// Initializes a new instance of the repo
    /// </summary>
    public MechanicRepository(AutoServiceContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public Task<List<Mechanic>> GetAllAsync()
    {
        return Task.FromResult(_context.Mechanics.ToList());
    }

    /// <inheritdoc />
    public Task<Mechanic?> GetByIdAsync(int id)
    {
        var mechanic = _context.Mechanics.FirstOrDefault(x => x.Id == id);
        return Task.FromResult(mechanic);
    }

    /// <inheritdoc />
    public Task AddAsync(Mechanic entity)
    {
        _context.Mechanics.Add(entity);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task UpdateAsync(Mechanic entity)
    {
        var existing = _context.Mechanics.FirstOrDefault(x => x.Id == entity.Id);

        if (existing is not null)
        {
            var index = _context.Mechanics.IndexOf(existing);
            _context.Mechanics[index] = entity;
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task DeleteAsync(Mechanic entity)
    {
        _context.Mechanics.Remove(entity);

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task<List<Mechanic>> GetBySpecializationAsync(MechanicSpecialization specialization)
    {
        var mechanics = _context.Mechanics.Where(mechanic => mechanic.Specialization == specialization).ToList();

        return Task.FromResult(mechanics);
    }

    /// <inheritdoc />
    public Task<bool> HasOrdersAsync(int mechanicId)
    {
        var hasOrders = _context.RepairOrders.Any(order => order.Mechanics.Any(orderMechanic => orderMechanic.MechanicId == mechanicId));

        return Task.FromResult(hasOrders);
    }
}