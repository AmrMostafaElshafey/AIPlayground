using AIPlayground.Api.Data;
using AIPlayground.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AIPlayground.Api.Controllers;

[ApiController]
[Route("api/requests")]
public class AccessRequestsController : ControllerBase
{
    private readonly AppDbContext _context;

    public AccessRequestsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AccessRequest>>> GetRequests()
    {
        var requests = await _context.AccessRequests.AsNoTracking().OrderByDescending(r => r.SubmittedAt).ToListAsync();
        return Ok(requests);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AccessRequest>> GetRequest(int id)
    {
        var request = await _context.AccessRequests.AsNoTracking().FirstOrDefaultAsync(item => item.Id == id);
        if (request is null)
        {
            return NotFound();
        }

        return Ok(request);
    }

    [HttpPost]
    public async Task<ActionResult<AccessRequest>> CreateRequest(AccessRequest request)
    {
        var service = await _context.Services.AsNoTracking().FirstOrDefaultAsync(item => item.Id == request.ServiceId);
        if (service is null)
        {
            return BadRequest("Service not found.");
        }

        if (service.IsApiAccess)
        {
            var limit = request.Role.Equals("Employee", StringComparison.OrdinalIgnoreCase)
                ? service.EmployeeTokenLimit
                : service.StudentTokenLimit;
            if (limit > 0 && request.EstimatedTokens > limit)
            {
                return BadRequest($"Estimated tokens exceed the allowed limit ({limit}).");
            }
        }

        request.SubmittedAt = DateTime.UtcNow;
        request.Status = "Pending";
        request.ServiceName = service.Name;
        _context.AccessRequests.Add(request);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetRequest), new { id = request.Id }, request);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<AccessRequest>> UpdateRequest(int id, AccessRequest request)
    {
        if (id != request.Id)
        {
            return BadRequest();
        }

        var existing = await _context.AccessRequests.FirstOrDefaultAsync(item => item.Id == id);
        if (existing is null)
        {
            return NotFound();
        }

        existing.RequesterName = request.RequesterName;
        existing.Role = request.Role;
        existing.ServiceId = request.ServiceId;
        existing.ServiceName = request.ServiceName;
        existing.ApplicationName = request.ApplicationName;
        existing.IntendedUse = request.IntendedUse;
        existing.Duration = request.Duration;
        existing.EstimatedTokens = request.EstimatedTokens;
        existing.Status = request.Status;
        existing.DecisionNotes = request.DecisionNotes;

        await _context.SaveChangesAsync();
        return Ok(existing);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteRequest(int id)
    {
        var request = await _context.AccessRequests.FirstOrDefaultAsync(item => item.Id == id);
        if (request is null)
        {
            return NotFound();
        }

        _context.AccessRequests.Remove(request);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
