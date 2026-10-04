using AutoService.Api.Services;
using AutoService.Contracts.DTOs;
using AutoService.Domain.Shared.Results;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AutoService.Api.Controllers;

/// <summary>
/// Defines REST endpoints for cars
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CarsController(CarService carService, ILogger<CarsController> logger) : ControllerBase
{
    /// <summary>
    /// Gets all cars
    /// </summary>
    [HttpGet]
    public ActionResult<List<CarDto>> GetAll()
    {
        logger.LogInformation("GET /api/cars");

        return Ok(carService.GetAll());
    }

    /// <summary>
    /// Gets a car by id
    /// </summary>
    [HttpGet("{id:int}")]
    public ActionResult<CarDto> GetById(int id)
    {
        logger.LogInformation("GET /api/cars/{CarId}", id);

        var car = carService.GetById(id);

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
    public ActionResult<List<CarDto>> GetByClientId(int clientId)
    {
        logger.LogInformation("GET /api/clients/{ClientId}/cars", clientId);

        var cars = carService.GetByClientId(clientId);

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
    public ActionResult<CarDto> Create(CreateCarDto dto)
    {
        logger.LogInformation("POST /api/cars");

        var car = carService.Create(dto);

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
    public ActionResult<CarDto> Update(int id, UpdateCarDto dto)
    {
        logger.LogInformation("PUT /api/cars/{CarId}", id);

        var car = carService.Update(id, dto);

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
    public IActionResult Delete(int id)
    {
        logger.LogInformation("DELETE /api/cars/{CarId}", id);

        var result = carService.Delete(id);

        return result switch
        {
            DeleteResult.Deleted => NoContent(),
            DeleteResult.NotFound => NotFound(),
            DeleteResult.HasRelatedEntities => Conflict(),
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }
}