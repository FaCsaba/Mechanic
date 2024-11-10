using MechanicBE.Errors;
using MechanicBE.Models;
using MechanicBE.Models.Dto;
using MechanicBE.ResultType;
using MechanicBE.Services;
using Microsoft.AspNetCore.Mvc;

namespace MechanicBE.Controllers;

[ApiController]
[Route("client")]
public class ClientController(IClientService clientService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Client>>> GetClients() =>
        Ok(await clientService.GetAllClientsAsync());

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<ActionResult<Client>> GetClient([FromRoute] Guid id) =>
        await clientService.EnsureClientExists(id);


    [HttpPost]
    public async Task<ActionResult<Client>> CreateClient([FromBody] CreateClient createClient) =>
        await clientService.CreateClientAsync(createClient);

    [HttpPut]
    public async Task<ActionResult?> UpdateClient([FromBody] UpdateClient updateClient) =>
        await clientService.UpdateClientAsync(updateClient);

    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<ActionResult?> DeleteClient([FromRoute] Guid id) => await clientService.DeleteClientAsync(id);
}