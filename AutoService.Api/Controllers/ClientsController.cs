using AutoService.Contracts.Dto;
using AutoService.Contracts.Interfaces;
using AutoService.Domain.Shared.Results;
using Microsoft.AspNetCore.Mvc;

namespace AutoService.Api.Controllers;

/// <summary>
/// Defines REST endpoints for clients
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ClientsController(IClientService clientService, ILogger<ClientsController> logger) : ControllerBase
{
    /// <summary>
    /// Gets all clients
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<ClientDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ClientDto>>> GetAll()
    {
        List<ClientDto> clients = await clientService.GetAllAsync();

        logger.LogInformation("Retrieved {Count} clients", clients.Count);

        return Ok(clients);
    }

    /// <summary>
    /// Gets a client by id
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ClientDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ClientDto>> GetById(int id)
    {
        ClientDto? client = await clientService.GetByIdAsync(id);

        if (client is null)
        {
            logger.LogWarning("Client with ID {ClientId} was not found", id);

            return NotFound();
        }
        logger.LogInformation("Client with ID {ClientId} was retrieved", id);

        return Ok(client);
    }

    /// <summary>
    /// Gets clients with more than one repair order during the last month
    /// </summary>
    [HttpGet("repeated")]
    [ProducesResponseType(typeof(List<RepeatedClientDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<RepeatedClientDto>>> GetRepeatedLastMonth()
    {
        List<RepeatedClientDto> repeatedClients = await clientService.GetRepeatedLastMonthAsync();

        logger.LogInformation("Retrieved {Count} repeated clients", repeatedClients.Count);

        return Ok(repeatedClients);
    }

    /// <summary>
    /// Creates a new client
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ClientDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ClientDto>> Create(UpsertClientDto dto)
    {
        ClientDto client = await clientService.CreateAsync(dto);

        logger.LogInformation("Client with ID {ClientId} was created", client.Id);

        return CreatedAtAction(nameof(GetById), new { id = client.Id }, client);
    }

    /// <summary>
    /// Updates an existing client
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ClientDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ClientDto>> Update(int id, UpsertClientDto dto)
    {
        UpdateResult<ClientDto> result = await clientService.UpdateAsync(id, dto);

        switch (result.Status)
        {
            case UpdateStatus.Success when result.Value is not null:
                logger.LogInformation("Client with ID {ClientId} was successfully updated", id);
                return Ok(result.Value);

            case UpdateStatus.NotFound:
                logger.LogWarning("Client with ID {ClientId} was not found for update", id);
                return NotFound();

            case UpdateStatus.Failed:
                logger.LogError("Failed to update client with ID {ClientId}", id);
                return Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Update failed",
                    detail: "The client could not be updated due to an internal error.");

            default:
                logger.LogError(
                    "Unexpected update result for client with ID {ClientId}: {Status}", id, result.Status);
                return Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Unexpected update result");
        }
    }

    /// <summary>
    /// Deletes a client
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int id)
    {
        DeleteResult result = await clientService.DeleteAsync(id);

        switch (result)
        {
            case DeleteResult.Deleted:
                logger.LogInformation("Client with ID {ClientId} was deleted", id);
                return NoContent();

            case DeleteResult.NotFound:
                logger.LogWarning("Client with ID {ClientId} was not found for deletion", id);
                return NotFound();

            case DeleteResult.HasRelatedEntities:
                logger.LogWarning("Client with ID {ClientId} has related entities and cannot be deleted", id);
                return Conflict();

            default:
                logger.LogError("An unexpected error occurred while deleting client with ID {ClientId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}