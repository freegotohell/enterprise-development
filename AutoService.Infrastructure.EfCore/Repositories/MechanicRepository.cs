using AutoService.Domain.Entities;
using AutoService.Domain.Interfaces.Repositories;
using AutoService.Domain.Shared.Enums;
using AutoService.Infrastructure.EfCore.Data;
using Microsoft.EntityFrameworkCore;

namespace AutoService.Infrastructure.EfCore.Repositories;

/// <summary>
/// Repository for mechanic data operations
/// </summary>
public class MechanicRepository(AutoServiceDbContext context) : IMechanicRepository
{
    /// <inheritdoc />
    public async Task<List<Mechanic>> GetAllAsync()
    {
        return await context.Mechanics.AsNoTracking().ToListAsync();
    }

    /// <inheritdoc />
    public async Task<Mechanic?> GetByIdAsync(int id)
    {
        return await context.Mechanics.FirstOrDefaultAsync(mechanic => mechanic.Id == id);
    }

    /// <inheritdoc />
    public async Task<Mechanic> AddAsync(Mechanic entity)
    {
        await context.Mechanics.AddAsync(entity);
        await context.SaveChangesAsync();

        return entity;
    }

    /// <inheritdoc />
    public async Task<bool> UpdateAsync(Mechanic entity)
    {
        context.Mechanics.Update(entity);

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
    public async Task<bool> DeleteAsync(Mechanic entity)
    {
        context.Mechanics.Remove(entity);

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
    public Task<List<Mechanic>> GetBySpecializationsAsync(IReadOnlyCollection<MechanicSpecialization> specializations)
    {
        return context.Mechanics.AsNoTracking().Where(mechanic => specializations.Contains(mechanic.Specialization)).ToListAsync();
    }

    /// <inheritdoc />
    public Task<bool> HasOrdersAsync(int mechanicId)
    {
        return context.OrderMechanics.AnyAsync(orderMechanic => orderMechanic.MechanicId == mechanicId);
    }

    /// <inheritdoc />
    public Task<List<Mechanic>> GetByIdsAsync(IReadOnlyCollection<int> ids)
    {
        return context.Mechanics.Where(mechanic => ids.Contains(mechanic.Id)).ToListAsync();
    }
}
