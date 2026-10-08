using AutoService.Domain.Entities;

namespace AutoService.Domain.Interfaces.Repositories;

/// <summary>
/// Data access operations for work types
/// </summary>
public interface IWorkTypeRepository : IRepository<WorkType>
{
    /// <summary>
    /// Checks if there are orders containing the work type
    /// </summary>
    public Task<bool> HasOrdersAsync(int workTypeId);
}