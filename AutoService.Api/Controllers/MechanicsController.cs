using AutoService.Api.Services;
using AutoService.Contracts.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace AutoService.Api.Controllers;

/// <summary>
/// Defines REST endpoints for mechanics
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class MechanicsController(MechanicService mechanicService, ILogger<MechanicsController> logger) : ControllerBase
{
    /// <summary>
    /// Gets all mechanics
    /// </summary>
    [HttpGet]
    public ActionResult<List<MechanicDto>> GetAll()
    {
        logger.LogInformation("GET /api/mechanics");

        return Ok(mechanicService.GetAll());
    }

    /// <summary>
    /// Gets a mechanic by id
    /// </summary>
    [HttpGet("{id:int}")]
    public ActionResult<MechanicDto> GetById(int id)
    {
        logger.LogInformation("GET /api/mechanics/{MechanicId}", id);

        var mechanic = mechanicService.GetById(id);

        if (mechanic is null)
        {
            return NotFound();
        }

        return Ok(mechanic);
    }

    /// <summary>
    /// Creates a new mechanic
    /// </summary>
    [HttpPost]
    public ActionResult<MechanicDto> Create(CreateMechanicDto dto)
    {
        logger.LogInformation("POST /api/mechanics");

        var mechanic = mechanicService.Create(dto);

        return CreatedAtAction(nameof(GetById), new { id = mechanic.Id }, mechanic);
    }

    /// <summary>
    /// Updates an existing mechanic
    /// </summary>
    [HttpPut("{id:int}")]
    public ActionResult<MechanicDto> Update(int id, UpdateMechanicDto dto)
    {
        logger.LogInformation("PUT /api/mechanics/{MechanicId}", id);

        var mechanic = mechanicService.Update(id, dto);

        if (mechanic is null)
        {
            return NotFound();
        }

        return Ok(mechanic);
    }

    /// <summary>
    /// Deletes a mechanic by id
    /// </summary>
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        logger.LogInformation("DELETE /api/mechanics/{MechanicId}", id);

        var deleted = mechanicService.Delete(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}