using AutoService.Api.Services;
using AutoService.Contracts.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace AutoService.Api.Controllers;

/// <summary>
/// Defines REST endpoints for clients
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ClientsController(ClientService clientService, ILogger<ClientsController> logger) : ControllerBase
{
    /// <summary>
    /// Gets all clients
    /// </summary>
    [HttpGet]
    public ActionResult<List<ClientDto>> GetAll()
    {
        logger.LogInformation("GET /api/clients");

        return Ok(clientService.GetAll());
    }

    /// <summary>
    /// Gets a client by id
    /// </summary>
    [HttpGet("{id:int}")]
    public ActionResult<ClientDto> GetById(int id)
    {
        logger.LogInformation("GET /api/clients/{ClientId}", id);

        var client = clientService.GetById(id);

        if (client is null)
        {
            return NotFound();
        }

        return Ok(client);
    }

    /// <summary>
    /// Creates a new client
    /// </summary>
    [HttpPost]
    public ActionResult<ClientDto> Create(CreateClientDto dto)
    {
        logger.LogInformation("POST /api/clients");

        var client = clientService.Create(dto);

        return CreatedAtAction(nameof(GetById), new { id = client.Id }, client);
    }

    /// <summary>
    /// Updates an existing client
    /// </summary>
    [HttpPut("{id:int}")]
    public ActionResult<ClientDto> Update(int id, UpdateClientDto dto)
    {
        logger.LogInformation("PUT /api/clients/{ClientId}", id);

        var client = clientService.Update(id, dto);

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
    public IActionResult Delete(int id)
    {
        logger.LogInformation("DELETE /api/clients/{ClientId}", id);

        var deleted = clientService.Delete(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}