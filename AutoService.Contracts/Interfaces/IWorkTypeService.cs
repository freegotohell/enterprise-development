using AutoService.Contracts.Dto;
using AutoService.Domain.Shared.Results;

namespace AutoService.Contracts.Interfaces;

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
    public Task<WorkTypeDto> CreateAsync(UpsertWorkTypeDto dto);

    /// <summary>
    /// Updates an existing work type
    /// </summary>
    public Task<UpdateResult<WorkTypeDto>> UpdateAsync(int id, UpsertWorkTypeDto dto);

    /// <summary>
    /// Deletes a work type by id
    /// </summary>
    public Task<DeleteResult> DeleteAsync(int id);
}