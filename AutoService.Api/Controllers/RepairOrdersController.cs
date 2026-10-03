using AutoService.Api.Services;
using AutoService.Contracts.DTOs;
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

        var deleted = repairOrderService.Delete(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
