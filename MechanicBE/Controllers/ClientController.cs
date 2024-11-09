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


    [HttpPost]
    public async Task<ActionResult<Client>> CreateClient([FromBody] CreateClient createClient) =>
        await clientService.CreateClientAsync(createClient);

    [HttpPut]
    public async Task<IActionResult> UpdateClient([FromBody] UpdateClient updateClient)
    {
        var res = await clientService.UpdateClientAsync(updateClient);
        return res is null ? Ok() : res.ToObjectResult();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteClient([FromQuery] Guid id)
    {
        var res = await clientService.DeleteClientAsync(id);
        return res is null ? Ok() : res.ToObjectResult();
    }
}