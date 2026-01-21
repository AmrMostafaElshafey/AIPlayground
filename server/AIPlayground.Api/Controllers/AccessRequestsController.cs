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

    [HttpPost]
    public async Task<ActionResult<AccessRequest>> CreateRequest(AccessRequest request)
    {
        request.SubmittedAt = DateTime.UtcNow;
        request.Status = "Pending";
        _context.AccessRequests.Add(request);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetRequests), new { id = request.Id }, request);
    }
}
