using AutoService.Contracts.DTOs;
using AutoService.Domain.Shared.Results;

namespace AutoService.Contracts.Services;

/// <summary>
/// Operations for managing work types
/// </summary>
public interface IWorkTypeService
{
    /// <summary>
    /// Gets all work types
    /// </summary>
    public Task<List<WorkTypeDto>> GetAllAsync();

    /// <summary>
    /// Gets a work type by id
    /// </summary>
    public Task<WorkTypeDto?> GetByIdAsync(int id);

    /// <summary>
    /// Gets five most frequently performed work types
    /// </summary>
    public Task<List<FrequentWorkTypeDto>> GetTop5MostFrequentAsync();

    /// <summary>
    /// Creates a new work type
    /// </summary>
    public Task<WorkTypeDto> CreateAsync(CreateWorkTypeDto dto);

    /// <summary>
    /// Updates an existing work type
    /// </summary>
    public Task<WorkTypeDto?> UpdateAsync(int id, UpdateWorkTypeDto dto);

    /// <summary>
    /// Deletes a work type by id
    /// </summary>
    public Task<DeleteResult> DeleteAsync(int id);
}