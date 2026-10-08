using AutoService.Contracts.DTOs;
using AutoService.Contracts.Services;
using AutoService.Domain.Shared.Enums;
using AutoService.Domain.Shared.Results;
using Microsoft.AspNetCore.Mvc;

namespace AutoService.Api.Controllers;

/// <summary>
/// Defines REST endpoints for mechanics
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class MechanicsController(IMechanicService mechanicService, ILogger<MechanicsController> logger) : ControllerBase
{
    /// <summary>
    /// Gets all mechanics
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<MechanicDto>>> GetAll()
    {
        logger.LogInformation("GET /api/mechanics");

        return Ok(await mechanicService.GetAllAsync());
    }

    /// <summary>
    /// Gets a mechanic by id
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<MechanicDto>> GetById(int id)
    {
        logger.LogInformation("GET /api/mechanics/{MechanicId}", id);

        MechanicDto? mechanic = await mechanicService.GetByIdAsync(id);

        if (mechanic is null)
        {
            return NotFound();
        }

        return Ok(mechanic);
    }

    /// <summary>
    /// Retrieves a list of clients associated with a specific mechanic
    /// </summary>
    [HttpGet("{id:int}/clients")]
    public async Task<ActionResult<List<ClientDto>>> GetClients(int id)
    {
        logger.LogInformation("GET /api/mechanics/{MechanicId}/clients", id);

        List<ClientDto>? clients = await mechanicService.GetClientsAsync(id);

        if (clients is null)
        {
            return NotFound();
        }

        return Ok(clients);
    }

    /// <summary>
    /// Gets mechanics associated with a specific work type
    /// </summary>
    [HttpGet("/api/worktypes/{workTypeId:int}/mechanics")]
    public async Task<ActionResult<List<MechanicDto>>> GetByWorkTypeId(int workTypeId)
    {
        logger.LogInformation("GET /api/worktypes/{WorkTypeId}/mechanics", workTypeId);

        List<MechanicDto>? mechanics = await mechanicService.GetByWorkTypeIdAsync(workTypeId);

        if (mechanics is null)
        {
            return NotFound();
        }

        return Ok(mechanics);
    }

    /// <summary>
    /// Gets mechanics by specialization
    /// </summary>
    [HttpGet("by-specialization")]
    public async Task<ActionResult<List<MechanicDto>>> GetBySpecialization([FromQuery] MechanicSpecialization specialization)
    {
        logger.LogInformation("GET /api/mechanics/by-specialization?specialization={Specialization}", specialization);

        return Ok(await mechanicService.GetBySpecializationAsync(specialization));
    }

    /// <summary>
    /// Creates a new mechanic
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<MechanicDto>> Create(CreateMechanicDto dto)
    {
        logger.LogInformation("POST /api/mechanics");

        MechanicDto mechanic = await mechanicService.CreateAsync(dto);

        return CreatedAtAction(nameof(GetById), new { id = mechanic.Id }, mechanic);
    }

    /// <summary>
    /// Updates an existing mechanic
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<MechanicDto>> Update(int id, UpdateMechanicDto dto)
    {
        logger.LogInformation("PUT /api/mechanics/{MechanicId}", id);

        MechanicDto? mechanic = await mechanicService.UpdateAsync(id, dto);

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
    public async Task<IActionResult> Delete(int id)
    {
        logger.LogInformation("DELETE /api/mechanics/{MechanicId}", id);

        DeleteResult result = await mechanicService.DeleteAsync(id);

        return result switch
        {
            DeleteResult.Deleted => NoContent(),
            DeleteResult.NotFound => NotFound(),
            DeleteResult.HasRelatedEntities => Conflict(),
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }
}