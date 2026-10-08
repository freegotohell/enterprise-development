using AutoService.Contracts.DTOs;
using AutoService.Contracts.Services;
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
    public async Task<ActionResult<List<WorkTypeDto>>> GetAll()
    {
        logger.LogInformation("GET /api/worktypes");

        return Ok(await workTypeService.GetAllAsync());
    }

    /// <summary>
    /// Gets a work type by id
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<WorkTypeDto>> GetById(int id)
    {
        logger.LogInformation("GET /api/worktypes/{WorkTypeId}", id);

        WorkTypeDto? workType = await workTypeService.GetByIdAsync(id);

        if (workType is null)
        {
            return NotFound();
        }

        return Ok(workType);
    }

    /// <summary>
    /// Gets the five most frequently performed work types
    /// </summary>
    [HttpGet("top5")]
    public async Task<ActionResult<List<FrequentWorkTypeDto>>> GetTop5MostFrequent()
    {
        logger.LogInformation("GET /api/worktypes/top5");

        return Ok(await workTypeService.GetTop5MostFrequentAsync());
    }

    /// <summary>
    /// Creates a new work type
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<WorkTypeDto>> Create(CreateWorkTypeDto dto)
    {
        logger.LogInformation("POST /api/worktypes");

        WorkTypeDto workType = await workTypeService.CreateAsync(dto);

        return CreatedAtAction(nameof(GetById), new { id = workType.Id }, workType);
    }

    /// <summary>
    /// Updates an existing work type
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<WorkTypeDto>> Update(int id, UpdateWorkTypeDto dto)
    {
        logger.LogInformation("PUT /api/worktypes/{WorkTypeId}", id);

        WorkTypeDto? workType = await workTypeService.UpdateAsync(id, dto);

        if (workType is null)
        {
            return NotFound();
        }

        return Ok(workType);
    }

    /// <summary>
    /// Deletes a work type by id
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        logger.LogInformation("DELETE /api/worktypes/{WorkTypeId}", id);

        DeleteResult result = await workTypeService.DeleteAsync(id);

        return result switch
        {
            DeleteResult.Deleted => NoContent(),
            DeleteResult.NotFound => NotFound(),
            DeleteResult.HasRelatedEntities => Conflict(),
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }
}
