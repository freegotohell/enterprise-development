using AutoService.Contracts.Dto;
using AutoService.Contracts.Interfaces;
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
    [ProducesResponseType(typeof(List<CarDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<CarDto>>> GetAll()
    {
        List<CarDto> cars = await carService.GetAllAsync();

        logger.LogInformation("Retrieved {Count} cars", cars.Count);

        return Ok(cars);
    }

    /// <summary>
    /// Gets a car by id
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CarDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CarDto>> GetById(int id)
    {
        CarDto? car = await carService.GetByIdAsync(id);

        if (car is null)
        {
            logger.LogWarning("Car with ID {CarId} was not found", id);

            return NotFound();
        }

        logger.LogInformation("Car with ID {CarId} was retrieved", id);

        return Ok(car);
    }

    /// <summary>
    /// Gets cars belonging to a client
    /// </summary>
    [HttpGet("/api/clients/{clientId:int}/cars")]
    [ProducesResponseType(typeof(List<CarDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<CarDto>>> GetByClientId(int clientId)
    {
        List<CarDto>? cars = await carService.GetByClientIdAsync(clientId);

        if (cars is null)
        {
            logger.LogWarning("Client with ID {ClientId} was not found when retrieving cars", clientId);

            return NotFound();
        }

        logger.LogInformation("Retrieved {Count} cars for client with ID {ClientId}", cars.Count, clientId);

        return Ok(cars);
    }

    /// <summary>
    /// Creates a new car
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(CarDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CarDto>> Create(UpsertCarDto dto)
    {
        CarDto? car = await carService.CreateAsync(dto);

        if (car is null)
        {
            logger.LogWarning("Car creation failed because the provided data was invalid");

            return BadRequest();
        }

        logger.LogInformation("Car with ID {CarId} was created", car.Id);

        return CreatedAtAction(nameof(GetById), new { id = car.Id }, car);
    }

    /// <summary>
    /// Updates an existing car
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(CarDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CarDto>> Update(int id, UpsertCarDto dto)
    {
        UpdateResult<CarDto> result = await carService.UpdateAsync(id, dto);

        switch (result.Status)
        {
            case UpdateStatus.Success when result.Value is not null:
                logger.LogInformation("Car with ID {CarId} was successfully updated", id);
                return Ok(result.Value);

            case UpdateStatus.NotFound:
                logger.LogWarning("Car with ID {CarId} was not found for update", id);
                return NotFound();

            case UpdateStatus.InvalidReference:
                logger.LogWarning("Cannot update car with ID {CarId}: client reference is invalid", id);
                return BadRequest(
                    "The specified client does not exist.");

            case UpdateStatus.Failed:
                logger.LogError("Failed to update car with ID {CarId}", id);
                return Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Update failed",
                    detail: "The car could not be updated due to an internal error.");

            default:
                logger.LogError("Unexpected update result for car with ID {CarId}: {Status}", id, result.Status);
                return Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Unexpected update result");
        }
    }

    /// <summary>
    /// Deletes a car
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int id)
    {
        DeleteResult result = await carService.DeleteAsync(id);

        switch (result)
        {
            case DeleteResult.Deleted:
                logger.LogInformation("Car with ID {CarId} was deleted", id);
                return NoContent();

            case DeleteResult.NotFound:
                logger.LogWarning("Car with ID {CarId} was not found for deletion", id);
                return NotFound();

            case DeleteResult.HasRelatedEntities:
                logger.LogWarning("Car with ID {CarId} has related entities and cannot be deleted", id);
                return Conflict();

            default:
                logger.LogError("An unexpected result occurred while deleting car with ID {CarId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}