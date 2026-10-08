using AutoService.Contracts.DTOs;
using AutoService.Contracts.Services;
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
    public async Task<ActionResult<List<ClientDto>>> GetAll()
    {
        logger.LogInformation("GET /api/clients");

        return Ok(await clientService.GetAllAsync());
    }

    /// <summary>
    /// Gets a client by id
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ClientDto>> GetById(int id)
    {
        logger.LogInformation("GET /api/clients/{ClientId}", id);

        ClientDto? client = await clientService.GetByIdAsync(id);

        if (client is null)
        {
            return NotFound();
        }

        return Ok(client);
    }

    /// <summary>
    /// Gets clients with more than one repair order during the last month
    /// </summary>
    [HttpGet("repeated")]
    public async Task<ActionResult<List<RepeatedClientDto>>> GetRepeatedLastMonth()
    {
        logger.LogInformation("GET /api/clients/repeated");

        return Ok(await clientService.GetRepeatedLastMonthAsync());
    }

    /// <summary>
    /// Creates a new client
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ClientDto>> Create(CreateClientDto dto)
    {
        logger.LogInformation("POST /api/clients");

        ClientDto client = await clientService.CreateAsync(dto);

        return CreatedAtAction(nameof(GetById), new { id = client.Id }, client);
    }

    /// <summary>
    /// Updates an existing client
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ClientDto>> Update(int id, UpdateClientDto dto)
    {
        logger.LogInformation("PUT /api/clients/{ClientId}", id);

        ClientDto? client = await clientService.UpdateAsync(id, dto);

        if (client is null)
        {
            return NotFound();
        }

        return Ok(client);
    }

    /// <summary>
    /// Deletes a client
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        logger.LogInformation("DELETE /api/clients/{ClientId}", id);

        DeleteResult result = await clientService.DeleteAsync(id);

        return result switch
        {
            DeleteResult.Deleted => NoContent(),
            DeleteResult.NotFound => NotFound(),
            DeleteResult.HasRelatedEntities => Conflict(),
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }
}