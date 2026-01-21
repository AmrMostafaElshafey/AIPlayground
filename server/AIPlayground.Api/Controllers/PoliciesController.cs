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

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PolicyDocument>> GetPolicy(int id)
    {
        var policy = await _context.Policies.AsNoTracking().FirstOrDefaultAsync(item => item.Id == id);
        if (policy is null)
        {
            return NotFound();
        }

        return Ok(policy);
    }

    [HttpPost]
    public async Task<ActionResult<PolicyDocument>> CreatePolicy(PolicyDocument policy)
    {
        policy.PublishedOn = policy.PublishedOn == default ? DateTime.UtcNow : policy.PublishedOn;
        _context.Policies.Add(policy);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetPolicy), new { id = policy.Id }, policy);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<PolicyDocument>> UpdatePolicy(int id, PolicyDocument policy)
    {
        if (id != policy.Id)
        {
            return BadRequest();
        }

        var existing = await _context.Policies.FirstOrDefaultAsync(item => item.Id == id);
        if (existing is null)
        {
            return NotFound();
        }

        existing.Title = policy.Title;
        existing.Version = policy.Version;
        existing.Summary = policy.Summary;
        existing.PublishedOn = policy.PublishedOn;

        await _context.SaveChangesAsync();
        return Ok(existing);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeletePolicy(int id)
    {
        var policy = await _context.Policies.FirstOrDefaultAsync(item => item.Id == id);
        if (policy is null)
        {
            return NotFound();
        }

        _context.Policies.Remove(policy);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
