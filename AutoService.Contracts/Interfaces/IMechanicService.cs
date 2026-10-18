using AutoService.Contracts.Dto;
using AutoService.Domain.Shared.Enums;
using AutoService.Domain.Shared.Results;

namespace AutoService.Contracts.Interfaces;

/// <summary>
/// Operations for managing mechanics
/// </summary>
public interface IMechanicService
{
    /// <summary>
    /// Gets all mechanics
    /// </summary>
    public Task<List<MechanicDto>> GetAllAsync();

    /// <summary>
    /// Gets a mechanic by id
    /// </summary>
    public Task<MechanicDto?> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves list of clients whose repair orders associated with a specific mechanic
    /// </summary>
    public Task<List<ClientDto>?> GetClientsAsync(int mechanicId);

    /// <summary>
    /// Gets mechanics suitable for a specific work type
    /// </summary>
    public Task<List<MechanicDto>?> GetByWorkTypeIdAsync(int workTypeId);

    /// <summary>
    /// Gets mechanics by specialization
    /// </summary>
    public Task<List<MechanicDto>> GetBySpecializationAsync(MechanicSpecialization specialization);

    /// <summary>
    /// Creates a new mechanic
    /// </summary>
    public Task<MechanicDto> CreateAsync(UpsertMechanicDto dto);

    /// <summary>
    /// Updates an existing mechanic
    /// </summary>
    public Task<UpdateResult<MechanicDto>> UpdateAsync(int id, UpsertMechanicDto dto);

    /// <summary>
    /// Deletes a mechanic by id
    /// </summary>
    public Task<DeleteResult> DeleteAsync(int id);
}