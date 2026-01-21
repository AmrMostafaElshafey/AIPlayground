using AIPlayground.Api.Data;
using AIPlayground.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AIPlayground.Api.Controllers;

[ApiController]
[Route("api/policies")]
public class PoliciesController : ControllerBase
{
    private readonly AppDbContext _context;

    public PoliciesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PolicyDocument>>> GetPolicies()
    {
        var policies = await _context.Policies.AsNoTracking().OrderByDescending(p => p.PublishedOn).ToListAsync();
        return Ok(policies);
    }
}
