using AutoService.Contracts.Dto;
using AutoService.Contracts.Interfaces;
using AutoService.Domain.Shared.Results;
using Microsoft.AspNetCore.Mvc;

namespace AutoService.Api.Controllers;

/// <summary>
/// Defines REST endpoints for work types
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class WorkTypesController(IWorkTypeService workTypeService, ILogger<WorkTypesController> logger) : ControllerBase
{
    /// <summary>
    /// Gets all work types
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<WorkTypeDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<WorkTypeDto>>> GetAll()
    {
        List<WorkTypeDto> workTypes = await workTypeService.GetAllAsync();

        logger.LogInformation("Retrieved {Count} work types", workTypes.Count);

        return Ok(workTypes);
    }

    /// <summary>
    /// Gets a work type by id
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(WorkTypeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WorkTypeDto>> GetById(int id)
    {
        WorkTypeDto? workType = await workTypeService.GetByIdAsync(id);

        if (workType is null)
        {
            logger.LogWarning("Work type with ID {WorkTypeId} was not found", id);

            return NotFound();
        }

        logger.LogInformation("Work type with ID {WorkTypeId} was retrieved", id);

        return Ok(workType);
    }

    /// <summary>
    /// Gets the five most frequently performed work types
    /// </summary>
    [HttpGet("top5")]
    [ProducesResponseType(typeof(List<FrequentWorkTypeDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<FrequentWorkTypeDto>>> GetTop5MostFrequent()
    {
        List<FrequentWorkTypeDto> workTypes = await workTypeService.GetTop5MostFrequentAsync();

        logger.LogInformation("Retrieved 5 most frequently performed work types");

        return Ok(workTypes);
    }

    /// <summary>
    /// Creates a new work type
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(WorkTypeDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<WorkTypeDto>> Create(UpsertWorkTypeDto dto)
    {
        WorkTypeDto workType = await workTypeService.CreateAsync(dto);

        logger.LogInformation("Work type with ID {WorkTypeId} was created", workType.Id);

        return CreatedAtAction(nameof(GetById), new { id = workType.Id }, workType);
    }

    /// <summary>
    /// Updates an existing work type
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(WorkTypeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<WorkTypeDto>> Update(int id, UpsertWorkTypeDto dto)
    {
        UpdateResult<WorkTypeDto> result = await workTypeService.UpdateAsync(id, dto);

        switch (result.Status)
        {
            case UpdateStatus.Success when result.Value is not null:
                logger.LogInformation("Work type with ID {WorkTypeId} was successfully updated", id);
                return Ok(result.Value);

            case UpdateStatus.NotFound:
                logger.LogWarning("Work type with ID {WorkTypeId} was not found for update", id);
                return NotFound();

            case UpdateStatus.Failed:
                logger.LogError("Failed to update work type with ID {WorkTypeId}", id);
                return Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Update failed",
                    detail: "The work type could not be updated due to an internal error.");

            default:
                logger.LogError(
                    "Unexpected update result for work type with ID {WorkTypeId}: {Status}", id, result.Status);
                return Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Unexpected update result");
        }
    }

    /// <summary>
    /// Deletes a work type by id
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int id)
    {
        DeleteResult result = await workTypeService.DeleteAsync(id);

        switch (result)
        {
            case DeleteResult.Deleted:
                logger.LogInformation("Work type with ID {WorkTypeId} was deleted", id);
                return NoContent();

            case DeleteResult.NotFound:
                logger.LogWarning("Work type with ID {WorkTypeId} was not found for deletion", id);
                return NotFound();

            case DeleteResult.HasRelatedEntities:
                logger.LogWarning("Work type with ID {WorkTypeId} has related entities and cannot be deleted", id);
                return Conflict();

            default:
                logger.LogError("An unexpected result occurred while deleting work type with ID {WorkTypeId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
