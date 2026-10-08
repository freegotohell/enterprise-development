using AutoService.Contracts.DTOs;
using AutoService.Domain.Shared.Results;

namespace AutoService.Contracts.Services;

/// <summary>
/// Operations for managing repair orders
/// </summary>
public interface IRepairOrderService
{
    /// <summary>
    /// Gets all repair orders
    /// </summary>
    public Task<List<RepairOrderDto>> GetAllAsync();

    /// <summary>
    /// Gets a repair order by id
    /// </summary>
    public Task<RepairOrderDto?> GetByIdAsync(int id);

    /// <summary>
    /// Gets all repair orders placed by the client
    /// </summary>
    public Task<List<RepairOrderDto>?> GetByClientIdAsync(int clientId);

    /// <summary>
    /// Gets all repair orders by car id
    /// </summary>
    public Task<List<RepairOrderDto>?> GetByCarIdAsync(int carId);

    /// <summary>
    /// Gets repair orders associated with the mechanic
    /// </summary>
    public Task<List<RepairOrderDto>?> GetByMechanicIdAsync(int mechanicId);

    /// <summary>
    /// Gets repair orders with the work type
    /// </summary>
    public Task<List<RepairOrderDto>?> GetByWorkTypeIdAsync(int workTypeId);

    /// <summary>
    /// Calculates the total cost of a repair order
    /// </summary>
    public Task<RepairOrderCostDto?> GetTotalCostAsync(int id);

    /// <summary>
    /// Creates a new repair order
    /// </summary>
    public Task<RepairOrderDto?> CreateAsync(CreateRepairOrderDto dto);

    /// <summary>
    /// Updates an existing repair order
    /// </summary>
    public Task<RepairOrderDto?> UpdateAsync(int id, UpdateRepairOrderDto dto);

    /// <summary>
    /// Deletes a repair order by id
    /// </summary>
    public Task<DeleteResult> DeleteAsync(int id);
}