using AutoService.Contracts.Dto;
using AutoService.Domain.Shared.Results;

namespace AutoService.Contracts.Interfaces;

/// <summary>
/// Operations for managing cars
/// </summary>
public interface ICarService
{
    /// <summary>
    /// Gets all cars
    /// </summary>
    public Task<List<CarDto>> GetAllAsync();

    /// <summary>
    /// Gets a car by id
    /// </summary>
    public Task<CarDto?> GetByIdAsync(int id);

    /// <summary>
    /// Creates a new car
    /// </summary>
    public Task<CarDto?> CreateAsync(UpsertCarDto dto);

    /// <summary>
    /// Updates an existing car
    /// </summary>
    public Task<UpdateResult<CarDto>> UpdateAsync(int id, UpsertCarDto dto);

    /// <summary>
    /// Deletes a car by id
    /// </summary>
    public Task<DeleteResult> DeleteAsync(int id);

    /// <summary>
    /// Gets cars by the client id
    /// </summary>
    public Task<List<CarDto>?> GetByClientIdAsync(int clientId);
}