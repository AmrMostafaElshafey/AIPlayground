using AIPlayground.Api.Data;
using AIPlayground.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AIPlayground.Api.Controllers;

[ApiController]
[Route("api/roles")]
public class RolesController : ControllerBase
{
    private readonly AppDbContext _context;

    public RolesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoleProfile>>> GetRoles()
    {
        var roles = await _context.Roles.AsNoTracking().ToListAsync();
        return Ok(roles);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<RoleProfile>> GetRole(int id)
    {
        var role = await _context.Roles.AsNoTracking().FirstOrDefaultAsync(item => item.Id == id);
        if (role is null)
        {
            return NotFound();
        }

        return Ok(role);
    }

    [HttpPost]
    public async Task<ActionResult<RoleProfile>> CreateRole(RoleProfile role)
    {
        _context.Roles.Add(role);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetRole), new { id = role.Id }, role);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<RoleProfile>> UpdateRole(int id, RoleProfile role)
    {
        if (id != role.Id)
        {
            return BadRequest();
        }

        var existing = await _context.Roles.FirstOrDefaultAsync(item => item.Id == id);
        if (existing is null)
        {
            return NotFound();
        }

        existing.Name = role.Name;
        existing.Responsibilities = role.Responsibilities;

        await _context.SaveChangesAsync();
        return Ok(existing);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteRole(int id)
    {
        var role = await _context.Roles.FirstOrDefaultAsync(item => item.Id == id);
        if (role is null)
        {
            return NotFound();
        }

        _context.Roles.Remove(role);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
