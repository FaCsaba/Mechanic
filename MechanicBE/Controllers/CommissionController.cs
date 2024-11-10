using MechanicBE.Errors;
using MechanicBE.Models;
using MechanicBE.Models.Dto;
using MechanicBE.Services;
using Microsoft.AspNetCore.Mvc;

namespace MechanicBE.Controllers;

[ApiController]
[Route("commission")]
public class CommissionController(ICommissionService commissionService, IWorkHourEstimator workHourEstimator)
    : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<CommissionDto>>> GetCommissions() =>
        (await commissionService.GetCommissionsAsync()).Select(CommissionToCommissionDto).ToList();

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<ActionResult<CommissionDto>> GetCommission(Guid id) =>
        (await commissionService.EnsureCommissionExists(id)).Map(CommissionToCommissionDto);

    [HttpPost]
    public async Task<ActionResult<CommissionDto>> CreateCommission([FromBody] CreateCommission createCommission) =>
        (await commissionService.CreateCommissionAsync(createCommission)).Map(CommissionToCommissionDto);

    [HttpPut]
    public async Task<ActionResult> UpdateCommission([FromBody] UpdateCommission updateCommission) =>
        await commissionService.UpdateCommissionAsync(updateCommission);

    [HttpDelete]
    public async Task<ActionResult> DeleteCommission([FromQuery] Guid id) =>
        await commissionService.DeleteCommissionAsync(id);

    private CommissionDto CommissionToCommissionDto(Commission commission) => new CommissionDto
    {
        Id = commission.Id,
        Client = commission.Client,
        Severity = commission.Severity,
        Status = commission.Status,
        Description = commission.Description,
        FaultCategory = commission.FaultCategory,
        LicensePlateNumber = commission.LicensePlateNumber,
        VehicleManufacturingDate = commission.VehicleManufacturingDate,
        WorkHourEstimate = workHourEstimator.WorkHours(commission),
    };
}