using AutoService.Contracts.DTOs;
using AutoService.Contracts.Services;
using AutoService.Domain.Shared.Results;
using Microsoft.AspNetCore.Mvc;

namespace AutoService.Api.Controllers;

/// <summary>
/// Defines REST endpoints for repair orders
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class RepairOrdersController(IRepairOrderService repairOrderService, ILogger<RepairOrdersController> logger) : ControllerBase
{
    /// <summary>
    /// Gets all repair orders
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<RepairOrderDto>>> GetAll()
    {
        logger.LogInformation("GET /api/repairorders");

        return Ok(await repairOrderService.GetAllAsync());
    }

    /// <summary>
    /// Gets a repair order by id
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<RepairOrderDto>> GetById(int id)
    {
        logger.LogInformation("GET /api/repairorders/{RepairOrderId}", id);

        RepairOrderDto? order = await repairOrderService.GetByIdAsync(id);

        if (order is null)
        {
            return NotFound();
        }

        return Ok(order);
    }

    /// <summary>
    /// Gets all repair orders placed by the client
    /// </summary>
    [HttpGet("/api/clients/{clientId:int}/repairorders")]
    public async Task<ActionResult<List<RepairOrderDto>>> GetByClientId(int clientId)
    {
        logger.LogInformation("GET /api/clients/{ClientId}/repairorders", clientId);

        List<RepairOrderDto>? orders = await repairOrderService.GetByClientIdAsync(clientId);

        if (orders is null)
        {
            return NotFound();
        }

        return Ok(orders);
    }

    /// <summary>
    /// Gets all repair orders associated with the car
    /// </summary>
    [HttpGet("/api/cars/{carId:int}/repairorders")]
    public async Task<ActionResult<List<RepairOrderDto>>> GetByCarId(int carId)
    {
        logger.LogInformation("GET /api/cars/{CarId}/repairorders", carId);

        List<RepairOrderDto>? orders = await repairOrderService.GetByCarIdAsync(carId);

        if (orders is null)
        {
            return NotFound();
        }

        return Ok(orders);
    }

    /// <summary>
    /// Gets repair orders associated with the mechanic
    /// </summary>
    [HttpGet("/api/mechanics/{mechanicId:int}/repairorders")]
    public async Task<ActionResult<List<RepairOrderDto>>> GetByMechanicId(int mechanicId)
    {
        logger.LogInformation("GET /api/mechanics/{MechanicId}/repairorders", mechanicId);

        List<RepairOrderDto>? orders = await repairOrderService.GetByMechanicIdAsync(mechanicId);

        if (orders is null)
        {
            return NotFound();
        }

        return Ok(orders);
    }

    /// <summary>
    /// Gets repair orders with the work type
    /// </summary>
    [HttpGet("/api/worktypes/{workTypeId:int}/repairorders")]
    public async Task<ActionResult<List<RepairOrderDto>>> GetByWorkTypeId(int workTypeId)
    {
        logger.LogInformation("GET /api/worktypes/{WorkTypeId}/repairorders", workTypeId);

        List<RepairOrderDto>? orders = await repairOrderService.GetByWorkTypeIdAsync(workTypeId);

        if (orders is null)
        {
            return NotFound();
        }

        return Ok(orders);
    }

    /// <summary>
    /// Gets the total cost of a repair order
    /// </summary>
    [HttpGet("{id:int}/totalcost")]
    public async Task<ActionResult<RepairOrderCostDto>> GetTotalCost(int id)
    {
        logger.LogInformation("GET /api/repairorders/{RepairOrderId}/totalcost", id);

        RepairOrderCostDto? result = await repairOrderService.GetTotalCostAsync(id);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    /// <summary>
    /// Creates a new repair order
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<RepairOrderDto>> Create(CreateRepairOrderDto dto)
    {
        logger.LogInformation("POST /api/repairorders");

        RepairOrderDto? order = await repairOrderService.CreateAsync(dto);

        if (order is null)
        {
            return BadRequest();
        }

        return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
    }

    /// <summary>
    /// Updates an existing repair order
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<RepairOrderDto>> Update(int id, UpdateRepairOrderDto dto)
    {
        logger.LogInformation("PUT /api/repairorders/{RepairOrderId}", id);

        RepairOrderDto? order = await repairOrderService.UpdateAsync(id, dto);

        if (order is null)
        {
            return NotFound();
        }

        return Ok(order);
    }

    /// <summary>
    /// Deletes a repair order by id
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        logger.LogInformation("DELETE /api/repairorders/{RepairOrderId}", id);

        DeleteResult result = await repairOrderService.DeleteAsync(id);

        return result switch
        {
            DeleteResult.Deleted => NoContent(),
            DeleteResult.NotFound => NotFound(),
            DeleteResult.HasRelatedEntities => Conflict(),
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }
}
