using AutoService.Contracts.Dto;
using AutoService.Contracts.Interfaces;
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
    [ProducesResponseType(typeof(List<MechanicDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<MechanicDto>>> GetAll()
    {
        List<MechanicDto> mechanics = await mechanicService.GetAllAsync();

        logger.LogInformation("Retrieved {Count} mechanics", mechanics.Count);

        return Ok(mechanics);
    }

    /// <summary>
    /// Gets a mechanic by id
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(MechanicDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MechanicDto>> GetById(int id)
    {
        MechanicDto? mechanic = await mechanicService.GetByIdAsync(id);

        if (mechanic is null)
        {
            logger.LogWarning("Mechanic with ID {MechanicId} was not found", id);

            return NotFound();
        }

        logger.LogInformation("Mechanic with ID {MechanicId} was retrieved", id);

        return Ok(mechanic);
    }

    /// <summary>
    /// Retrieves a list of clients associated with a specific mechanic
    /// </summary>
    [HttpGet("{id:int}/clients")]
    [ProducesResponseType(typeof(List<ClientDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<ClientDto>>> GetClients(int id)
    {
        List<ClientDto>? clients = await mechanicService.GetClientsAsync(id);

        if (clients is null)
        {
            logger.LogWarning("Mechanic with ID {MechanicId} was not found when retrieving clients", id);

            return NotFound();
        }

        logger.LogInformation("Retrieved {Count} clients for mechanic with ID {MechanicId}", clients.Count, id);

        return Ok(clients);
    }

    /// <summary>
    /// Gets mechanics associated with a specific work type
    /// </summary>
    [HttpGet("/api/worktypes/{workTypeId:int}/mechanics")]
    [ProducesResponseType(typeof(List<MechanicDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<MechanicDto>>> GetByWorkTypeId(int workTypeId)
    {
        List<MechanicDto>? mechanics = await mechanicService.GetByWorkTypeIdAsync(workTypeId);

        if (mechanics is null)
        {
            logger.LogWarning("Work type with ID {WorkTypeId} was not found when retrieving mechanics", workTypeId);

            return NotFound();
        }

        logger.LogInformation("Retrieved {Count} mechanics for work type with ID {WorkTypeId}", mechanics.Count, workTypeId);

        return Ok(mechanics);
    }

    /// <summary>
    /// Gets mechanics by specialization
    /// </summary>
    [HttpGet("by-specialization")]
    [ProducesResponseType(typeof(List<MechanicDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<List<MechanicDto>>> GetBySpecialization([FromQuery] MechanicSpecialization specialization)
    {
        List<MechanicDto> mechanics = await mechanicService.GetBySpecializationAsync(specialization);

        logger.LogInformation("Retrieved {Count} mechanics with specialization {Specialization}", mechanics.Count, specialization);

        return Ok(mechanics);
    }

    /// <summary>
    /// Creates a new mechanic
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(MechanicDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MechanicDto>> Create(UpsertMechanicDto dto)
    {
        MechanicDto mechanic = await mechanicService.CreateAsync(dto);

        logger.LogInformation("Mechanic with ID {MechanicId} was created", mechanic.Id);

        return CreatedAtAction(nameof(GetById), new { id = mechanic.Id }, mechanic);
    }

    /// <summary>
    /// Updates an existing mechanic
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(MechanicDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<MechanicDto>> Update(int id, UpsertMechanicDto dto)
    {
        UpdateResult<MechanicDto> result = await mechanicService.UpdateAsync(id, dto);

        switch (result.Status)
        {
            case UpdateStatus.Success when result.Value is not null:
                logger.LogInformation("Mechanic with ID {MechanicId} was successfully updated", id);
                return Ok(result.Value);

            case UpdateStatus.NotFound:
                logger.LogWarning("Mechanic with ID {MechanicId} was not found for update", id);
                return NotFound();

            case UpdateStatus.Failed:
                logger.LogError("Failed to update mechanic with ID {MechanicId}", id);
                return Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Update failed",
                    detail: "The mechanic could not be updated due to an internal error.");

            default:
                logger.LogError(
                    "Unexpected update result for mechanic with ID {MechanicId}: {Status}", id, result.Status);
                return Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Unexpected update result");
        }
    }

    /// <summary>
    /// Deletes a mechanic by id
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int id)
    {
        DeleteResult result = await mechanicService.DeleteAsync(id);

        switch (result)
        {
            case DeleteResult.Deleted:
                logger.LogInformation("Mechanic with ID {MechanicId} was deleted", id);
                return NoContent();

            case DeleteResult.NotFound:
                logger.LogWarning("Mechanic with ID {MechanicId} was not found for deletion", id);
                return NotFound();

            case DeleteResult.HasRelatedEntities:
                logger.LogWarning("Mechanic with ID {MechanicId} has related entities and cannot be deleted", id);
                return Conflict();

            default:
                logger.LogError("An unexpected result occurred while deleting mechanic with ID {MechanicId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}