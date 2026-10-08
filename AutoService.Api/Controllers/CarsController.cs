using AutoService.Contracts.DTOs;
using AutoService.Contracts.Services;
using AutoService.Domain.Shared.Results;
using Microsoft.AspNetCore.Mvc;

namespace AutoService.Api.Controllers;

/// <summary>
/// Defines REST endpoints for cars
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CarsController(ICarService carService, ILogger<CarsController> logger) : ControllerBase
{
    /// <summary>
    /// Gets all cars
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<CarDto>>> GetAll()
    {
        logger.LogInformation("GET /api/cars");

        return Ok(await carService.GetAllAsync());
    }

    /// <summary>
    /// Gets a car by id
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<CarDto>> GetById(int id)
    {
        logger.LogInformation("GET /api/cars/{CarId}", id);

        CarDto? car = await carService.GetByIdAsync(id);

        if (car is null)
        {
            return NotFound();
        }

        return Ok(car);
    }

    /// <summary>
    /// Gets cars belonging to a client
    /// </summary>
    [HttpGet("/api/clients/{clientId:int}/cars")]
    public async Task<ActionResult<List<CarDto>>> GetByClientId(int clientId)
    {
        logger.LogInformation("GET /api/clients/{ClientId}/cars", clientId);

        List<CarDto>? cars = await carService.GetByClientIdAsync(clientId);

        if (cars is null)
        {
            return NotFound();
        }

        return Ok(cars);
    }

    /// <summary>
    /// Creates a new car
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<CarDto>> Create(CreateCarDto dto)
    {
        logger.LogInformation("POST /api/cars");

        CarDto? car = await carService.CreateAsync(dto);

        if (car is null)
        {
            return BadRequest();
        }

        return CreatedAtAction(nameof(GetById), new { id = car.Id }, car);
    }

    /// <summary>
    /// Updates an existing car
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<CarDto>> Update(int id, UpdateCarDto dto)
    {
        logger.LogInformation("PUT /api/cars/{CarId}", id);

        CarDto? car = await carService.UpdateAsync(id, dto);

        if (car is null)
        {
            return NotFound();
        }

        return Ok(car);
    }

    /// <summary>
    /// Deletes a car
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        logger.LogInformation("DELETE /api/cars/{CarId}", id);

        DeleteResult result = await carService.DeleteAsync(id);

        return result switch
        {
            DeleteResult.Deleted => NoContent(),
            DeleteResult.NotFound => NotFound(),
            DeleteResult.HasRelatedEntities => Conflict(),
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }
}