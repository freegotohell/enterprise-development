using AutoService.Api.Services;
using AutoService.Contracts.DTOs;
using AutoService.Domain.Shared.Enums;
using AutoService.Domain.Shared.Results;
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

        MechanicDto? mechanic = mechanicService.GetById(id);

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
    public ActionResult<List<ClientDto>> GetClients(int id)
    {
        logger.LogInformation("GET /api/mechanics/{MechanicId}/clients", id);

        List<ClientDto>? clients = mechanicService.GetClients(id);

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
    public ActionResult<List<MechanicDto>> GetByWorkTypeId(int workTypeId)
    {
        logger.LogInformation("GET /api/worktypes/{WorkTypeId}/mechanics", workTypeId);

        List<MechanicDto>? mechanics = mechanicService.GetByWorkTypeId(workTypeId);

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
    public ActionResult<List<MechanicDto>> GetBySpecialization([FromQuery] MechanicSpecialization specialization)
    {
        logger.LogInformation("GET /api/mechanics/by-specialization?specialization={Specialization}", specialization);

        return Ok(mechanicService.GetBySpecialization(specialization));
    }

    /// <summary>
    /// Creates a new mechanic
    /// </summary>
    [HttpPost]
    public ActionResult<MechanicDto> Create(CreateMechanicDto dto)
    {
        logger.LogInformation("POST /api/mechanics");

        MechanicDto mechanic = mechanicService.Create(dto);

        return CreatedAtAction(nameof(GetById), new { id = mechanic.Id }, mechanic);
    }

    /// <summary>
    /// Updates an existing mechanic
    /// </summary>
    [HttpPut("{id:int}")]
    public ActionResult<MechanicDto> Update(int id, UpdateMechanicDto dto)
    {
        logger.LogInformation("PUT /api/mechanics/{MechanicId}", id);

        MechanicDto? mechanic = mechanicService.Update(id, dto);

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

        DeleteResult result = mechanicService.Delete(id);

        return result switch
        {
            DeleteResult.Deleted => NoContent(),
            DeleteResult.NotFound => NotFound(),
            DeleteResult.HasRelatedEntities => Conflict(),
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }
}