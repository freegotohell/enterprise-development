using AutoService.Domain.Data;
using AutoService.Domain.Entities;
using AutoService.Domain.Interfaces.Repositories;
using AutoService.Domain.Shared.Enums;

namespace AutoService.Infrastructure.InMemory.Repositories;

/// <summary>
/// Repository for mechanics data operations
/// </summary>
public class MechanicRepository(AutoServiceContext context) : IMechanicRepository
{
    private int nextId = context.Mechanics.Count == 0 ? 1 : context.Mechanics.Max(mechanic => mechanic.Id) + 1;

    /// <inheritdoc />
    public Task<List<Mechanic>> GetAllAsync()
    {
        return Task.FromResult(context.Mechanics.ToList());
    }

    /// <inheritdoc />
    public Task<Mechanic?> GetByIdAsync(int id)
    {
        Mechanic? mechanic = context.Mechanics.FirstOrDefault(x => x.Id == id);

        return Task.FromResult(mechanic);
    }

    /// <inheritdoc />
    public Task<Mechanic> AddAsync(Mechanic entity)
    {
        if (context.Mechanics.Contains(entity))
        {
            return Task.FromResult(entity);
        }

        entity.Id = nextId++;
        context.Mechanics.Add(entity);

        return Task.FromResult(entity);
    }

    /// <inheritdoc />
    public Task<bool> UpdateAsync(Mechanic entity)
    {
        var index = context.Mechanics.FindIndex(mechanic => mechanic.Id == entity.Id);

        if (index == -1)
        {
            return Task.FromResult(false);
        }

        context.Mechanics[index] = entity;

        return Task.FromResult(true);
    }

    /// <inheritdoc />
    public Task<bool> DeleteAsync(Mechanic entity)
    {
        Mechanic? existing = context.Mechanics.FirstOrDefault(mechanic => mechanic.Id == entity.Id);

        if (existing is null)
        {
            return Task.FromResult(false);
        }

        context.Mechanics.Remove(existing);

        return Task.FromResult(true);
    }

    /// <inheritdoc />
    public Task<List<Mechanic>> GetBySpecializationsAsync(IReadOnlyCollection<MechanicSpecialization> specializations)
    {
        var mechanics = context.Mechanics.Where(mechanic => specializations.Contains(mechanic.Specialization)).ToList();

        return Task.FromResult(mechanics);
    }

    /// <inheritdoc />
    public Task<bool> HasOrdersAsync(int mechanicId)
    {
        var hasOrders = context.RepairOrders.Any(order => order.Mechanics.Any(orderMechanic => orderMechanic.MechanicId == mechanicId));

        return Task.FromResult(hasOrders);
    }

    /// <inheritdoc />
    public Task<List<Mechanic>> GetByIdsAsync(IReadOnlyCollection<int> ids)
    {
        var mechanics = context.Mechanics.Where(mechanic => ids.Contains(mechanic.Id)).ToList();

        return Task.FromResult(mechanics);
    }
}