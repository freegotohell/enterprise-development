using AutoService.Api.Services;
using AutoService.Contracts.DTOs;
using AutoService.Domain.Shared.Results;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AutoService.Api.Controllers;

/// <summary>
/// Defines REST endpoints for repair orders
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class RepairOrdersController(RepairOrderService repairOrderService, ILogger<RepairOrdersController> logger) : ControllerBase
{
    /// <summary>
    /// Gets all repair orders
    /// </summary
    [HttpGet]
    public ActionResult<List<RepairOrderDto>> GetAll()
    {
        logger.LogInformation("GET /api/repairorders");

        return Ok(repairOrderService.GetAll());
    }

    /// <summary>
    /// Gets a repair order by id
    /// </summary>
    [HttpGet("{id:int}")]
    public ActionResult<RepairOrderDto> GetById(int id)
    {
        logger.LogInformation("GET /api/repairorders/{RepairOrderId}", id);

        var order = repairOrderService.GetById(id);

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
    public ActionResult<List<RepairOrderDto>> GetByClientId(int clientId)
    {
        logger.LogInformation("GET /api/clients/{ClientId}/repairorders", clientId);

        var orders = repairOrderService.GetByClientId(clientId);

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
    public ActionResult<List<RepairOrderDto>> GetByCarId(int carId)
    {
        logger.LogInformation("GET /api/cars/{CarId}/repairorders", carId);

        var orders = repairOrderService.GetByCarId(carId);

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
    public ActionResult<List<RepairOrderDto>> GetByMechanicId(int mechanicId)
    {
        logger.LogInformation("GET /api/mechanics/{MechanicId}/repairorders", mechanicId);

        var orders = repairOrderService.GetByMechanicId(mechanicId);

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
    public ActionResult<List<RepairOrderDto>> GetByWorkTypeId(int workTypeId)
    {
        logger.LogInformation("GET /api/worktypes/{WorkTypeId}/repairorders", workTypeId);

        var orders = repairOrderService.GetByWorkTypeId(workTypeId);

        if (orders is null)
        {
            return NotFound();
        }

        return Ok(orders);
    }

    /// <summary>
    /// Creates a new repair order
    /// </summary>
    [HttpPost]
    public ActionResult<RepairOrderDto> Create(CreateRepairOrderDto dto)
    {
        logger.LogInformation("POST /api/repairorders");

        var order = repairOrderService.Create(dto);

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
    public ActionResult<RepairOrderDto> Update(int id, UpdateRepairOrderDto dto)
    {
        logger.LogInformation("PUT /api/repairorders/{RepairOrderId}", id);

        var order = repairOrderService.Update(id, dto);

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
    public IActionResult Delete(int id)
    {
        logger.LogInformation("DELETE /api/repairorders/{RepairOrderId}", id);

        var result = repairOrderService.Delete(id);

        return result switch
        {
            DeleteResult.Deleted => NoContent(),
            DeleteResult.NotFound => NotFound(),
            DeleteResult.HasRelatedEntities => Conflict(),
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }
}
