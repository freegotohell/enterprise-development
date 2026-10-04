using AutoService.Api.Services;
using AutoService.Contracts.DTOs;
using AutoService.Domain.Shared.Results;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AutoService.Api.Controllers;

/// <summary>
/// Defines REST endpoints for work types
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class WorkTypesController(WorkTypeService workTypeService, ILogger<WorkTypesController> logger) : ControllerBase
{
    /// <summary>
    /// Gets all work types
    /// </summary>
    [HttpGet]
    public ActionResult<List<WorkTypeDto>> GetAll()
    {
        logger.LogInformation("GET /api/worktypes");

        return Ok(workTypeService.GetAll());
    }

    /// <summary>
    /// Gets a work type by id
    /// </summary>
    [HttpGet("{id:int}")]
    public ActionResult<WorkTypeDto> GetById(int id)
    {
        logger.LogInformation("GET /api/worktypes/{WorkTypeId}", id);

        var workType = workTypeService.GetById(id);

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
    public ActionResult<List<FrequentWorkTypeDto>> GetTop5MostFrequent()
    {
        logger.LogInformation("GET /api/worktypes/top5");

        return Ok(workTypeService.GetTop5MostFrequent());
    }

    /// <summary>
    /// Creates a new work type
    /// </summary>
    [HttpPost]
    public ActionResult<WorkTypeDto> Create(CreateWorkTypeDto dto)
    {
        logger.LogInformation("POST /api/worktypes");

        var workType = workTypeService.Create(dto);

        if (workType is null)
        {
            return BadRequest();
        }

        return CreatedAtAction(nameof(GetById), new { id = workType.Id }, workType);
    }

    /// <summary>
    /// Updates an existing work type
    /// </summary>
    [HttpPut("{id:int}")]
    public ActionResult<WorkTypeDto> Update(int id, UpdateWorkTypeDto dto)
    {
        logger.LogInformation("PUT /api/worktypes/{WorkTypeId}", id);

        var workType = workTypeService.Update(id, dto);

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
    public IActionResult Delete(int id)
    {
        logger.LogInformation("DELETE /api/worktypes/{WorkTypeId}", id);

        var result = workTypeService.Delete(id);

        return result switch
        {
            DeleteResult.Deleted => NoContent(),
            DeleteResult.NotFound => NotFound(),
            DeleteResult.HasRelatedEntities => Conflict(),
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }
}
