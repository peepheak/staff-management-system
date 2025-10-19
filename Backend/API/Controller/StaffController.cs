using API.Interfaces;
using API.Request;
using Microsoft.AspNetCore.Mvc;

namespace API.Controller;

[ApiController]
[Route("api/v1")]
public class StaffController(IStaffService service) : ControllerBase
{
    [HttpGet("staffs")]
    public async Task<IActionResult> GetAllAsync(string? staffId, int? gender, DateOnly? from,
        DateOnly? to, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var result = await service.GetAllAsync(staffId, gender, from, to, pageNumber, pageSize,
            cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(result);
        return Ok(result);
    }

    [HttpGet("staff/{id}")]
    public async Task<IActionResult> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        var result = await service.GetByIdAsync(id, cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(result);
        return Ok(result);
    }

    [HttpPost("staff")]
    public async Task<IActionResult> AddAsync(StaffAddRequest request, CancellationToken cancellationToken)
    {
        var result = await service.AddAsync(request, cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(result);
        return Ok(result);
    }

    [HttpPut("staff")]
    public async Task<IActionResult> UpdateAsync(StaffUpdateRequest request, CancellationToken cancellationToken)
    {
        var result = await service.UpdateAsync(request, cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(result);
        return Ok(result);
    }

    [HttpDelete("staff/{id}")]
    public async Task<IActionResult> DeleteAsync(string id, CancellationToken cancellationToken)
    {
        var result = await service.DeleteAsync(id, cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(result);
        return Ok(result);
    }

    [HttpGet("staffs/pdf")]
    public async Task<IActionResult> ExportToPdfAsync(int pageNumber, int pageSize, string? staffId, int? gender,
        DateOnly? from, DateOnly? to, CancellationToken cancellationToken)
    {
        var result = await service.ExportToPdfAsync(pageNumber, pageSize, staffId, gender, from, to,
            cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(result);
        return File(result.Data!, "application/pdf");
    }

    [HttpGet("staffs/excel")]
    public async Task<IActionResult> ExportToExcelAsync(int pageNumber, int pageSize, string? staffId, int? gender,
        DateOnly? from, DateOnly? to, CancellationToken cancellationToken)
    {
        var result = await service.ExportToExcelAsync(pageNumber, pageSize, staffId, gender, from, to,
            cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(result);
        return File(result.Data!, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }
}