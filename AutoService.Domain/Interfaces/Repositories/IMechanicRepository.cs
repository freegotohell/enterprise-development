using AutoService.Domain.Entities;
using AutoService.Domain.Shared.Enums;

namespace AutoService.Domain.Interfaces.Repositories;

/// <summary>
/// Data access operations for mechanics
/// </summary>
public interface IMechanicRepository : IRepository<Mechanic>
{
    /// <summary>
    /// Gets mechanics with the specified specialization
    /// </summary>
    public Task<List<Mechanic>> GetBySpecializationAsync(MechanicSpecialization specialization);

    /// <summary>
    /// Checks if the mechanic has orders assigned to him
    /// </summary>
    public Task<bool> HasOrdersAsync(int mechanicId);
}