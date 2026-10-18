using AutoService.Contracts.Dto;
using AutoService.Contracts.Interfaces;
using AutoService.Domain.Shared.Results;
using Microsoft.AspNetCore.Mvc;

namespace AutoService.Api.Controllers;

/// <summary>
/// Defines REST endpoints for repair orders
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class RepairOrdersController(IRepairOrderService repairOrderService, ILogger<RepairOrdersController> logger) : ControllerBase
{
    /// <summary>
    /// Gets all repair orders
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<RepairOrderDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<RepairOrderDto>>> GetAll()
    {
        List<RepairOrderDto> orders = await repairOrderService.GetAllAsync();

        logger.LogInformation("Retrieved {Count} repair orders", orders.Count);

        return Ok(orders);
    }

    /// <summary>
    /// Gets a repair order by id
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(RepairOrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RepairOrderDto>> GetById(int id)
    {
        RepairOrderDto? order = await repairOrderService.GetByIdAsync(id);

        if (order is null)
        {
            logger.LogWarning("Repair order with ID {RepairOrderId} was not found", id);

            return NotFound();
        }

        logger.LogInformation("Repair order with ID {RepairOrderId} was retrieved", id);

        return Ok(order);
    }

    /// <summary>
    /// Gets all repair orders placed by the client
    /// </summary>
    [HttpGet("/api/clients/{clientId:int}/repairorders")]
    [ProducesResponseType(typeof(List<RepairOrderDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<RepairOrderDto>>> GetByClientId(int clientId)
    {
        List<RepairOrderDto>? orders = await repairOrderService.GetByClientIdAsync(clientId);

        if (orders is null)
        {
            logger.LogWarning("Client with ID {ClientId} was not found when retrieving repair orders", clientId);

            return NotFound();
        }

        logger.LogInformation("Retrieved {Count} repair orders for client with ID {ClientId}", orders.Count, clientId);

        return Ok(orders);
    }

    /// <summary>
    /// Gets all repair orders associated with the car
    /// </summary>
    [HttpGet("/api/cars/{carId:int}/repairorders")]
    [ProducesResponseType(typeof(List<RepairOrderDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<RepairOrderDto>>> GetByCarId(int carId)
    {
        List<RepairOrderDto>? orders = await repairOrderService.GetByCarIdAsync(carId);

        if (orders is null)
        {
            logger.LogWarning("Car with ID {CarId} was not found when retrieving repair orders", carId);

            return NotFound();
        }

        logger.LogInformation("Retrieved {Count} repair orders for car with ID {CarId}", orders.Count, carId);

        return Ok(orders);
    }

    /// <summary>
    /// Gets repair orders associated with the mechanic
    /// </summary>
    [HttpGet("/api/mechanics/{mechanicId:int}/repairorders")]
    [ProducesResponseType(typeof(List<RepairOrderDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<RepairOrderDto>>> GetByMechanicId(int mechanicId)
    {
        List<RepairOrderDto>? orders = await repairOrderService.GetByMechanicIdAsync(mechanicId);

        if (orders is null)
        {
            logger.LogWarning("Mechanic with ID {MechanicId} was not found when retrieving repair orders", mechanicId);

            return NotFound();
        }

        logger.LogInformation("Retrieved {Count} repair orders for mechanic with ID {MechanicId}", orders.Count, mechanicId);

        return Ok(orders);
    }

    /// <summary>
    /// Gets repair orders with the work type
    /// </summary>
    [HttpGet("/api/worktypes/{workTypeId:int}/repairorders")]
    [ProducesResponseType(typeof(List<RepairOrderDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<RepairOrderDto>>> GetByWorkTypeId(int workTypeId)
    {
        List<RepairOrderDto>? orders = await repairOrderService.GetByWorkTypeIdAsync(workTypeId);

        if (orders is null)
        {
            logger.LogWarning("Work type with ID {WorkTypeId} was not found when retrieving repair orders", workTypeId);

            return NotFound();
        }

        logger.LogInformation("Retrieved {Count} repair orders for work type with ID {WorkTypeId}", orders.Count, workTypeId);

        return Ok(orders);
    }

    /// <summary>
    /// Gets the total cost of a repair order
    /// </summary>
    [HttpGet("{id:int}/totalcost")]
    [ProducesResponseType(typeof(RepairOrderCostDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RepairOrderCostDto>> GetTotalCost(int id)
    {
        RepairOrderCostDto? result = await repairOrderService.GetTotalCostAsync(id);

        if (result is null)
        {
            logger.LogWarning("Repair order with ID {RepairOrderId} was not found when calculating total cost", id);

            return NotFound();
        }

        logger.LogInformation("Retrieved total cost for repair order with ID {RepairOrderId}", id);

        return Ok(result);
    }

    /// <summary>
    /// Creates a new repair order
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(RepairOrderDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RepairOrderDto>> Create(UpsertRepairOrderDto dto)
    {
        RepairOrderDto? order = await repairOrderService.CreateAsync(dto);

        if (order is null)
        {
            logger.LogWarning("Repair order creation failed because the provided data was invalid");

            return BadRequest();
        }

        logger.LogInformation("Repair order with ID {RepairOrderId} was created", order.Id);

        return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
    }

    /// <summary>
    /// Updates an existing repair order
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(RepairOrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RepairOrderDto>> Update(int id, UpsertRepairOrderDto dto)
    {
        UpdateResult<RepairOrderDto> result = await repairOrderService.UpdateAsync(id, dto);

        switch (result.Status)
        {
            case UpdateStatus.Success when result.Value is not null:
                logger.LogInformation("Repair order with ID {RepairOrderId} was successfully updated", id);
                return Ok(result.Value);

            case UpdateStatus.NotFound:
                logger.LogWarning("Repair order with ID {RepairOrderId} was not found for update", id);
                return NotFound();

            case UpdateStatus.InvalidReference:
                logger.LogWarning("Cannot update repair order with ID {RepairOrderId}: one or more references are invalid", id);
                return BadRequest("The specified client, car, mechanic, or work type is invalid, or the car does not belong to the client.");

            case UpdateStatus.Failed:
                logger.LogError("Failed to update repair order with ID {RepairOrderId}", id);
                return Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Update failed",
                    detail: "The repair order could not be updated due to an internal error.");

            default:
                logger.LogError("Unexpected update result for repair order with ID {RepairOrderId}: {Status}", id, result.Status);
                return Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Unexpected update result");
        }
    }

    /// <summary>
    /// Deletes a repair order by id
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int id)
    {
        DeleteResult result = await repairOrderService.DeleteAsync(id);

        switch (result)
        {
            case DeleteResult.Deleted:
                logger.LogInformation("Repair order with ID {RepairOrderId} was deleted", id);
                return NoContent();

            case DeleteResult.NotFound:
                logger.LogWarning("Repair order with ID {RepairOrderId} was not found for deletion", id);
                return NotFound();

            case DeleteResult.HasRelatedEntities:
                logger.LogWarning("Repair order with ID {RepairOrderId} has related entities and cannot be deleted", id);
                return Conflict();

            default:
                logger.LogError("An unexpected result occurred while deleting repair order with ID {RepairOrderId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
