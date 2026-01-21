using AIPlayground.Api.Data;
using AIPlayground.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AIPlayground.Api.Controllers;

[ApiController]
[Route("api/services")]
public class ServicesController : ControllerBase
{
    private readonly AppDbContext _context;

    public ServicesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ServiceCatalogItem>>> GetServices()
    {
        var services = await _context.Services.AsNoTracking().ToListAsync();
        return Ok(services);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ServiceCatalogItem>> GetService(int id)
    {
        var service = await _context.Services.AsNoTracking().FirstOrDefaultAsync(item => item.Id == id);
        if (service is null)
        {
            return NotFound();
        }

        return Ok(service);
    }

    [HttpPost]
    public async Task<ActionResult<ServiceCatalogItem>> CreateService(ServiceCatalogItem service)
    {
        _context.Services.Add(service);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetService), new { id = service.Id }, service);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ServiceCatalogItem>> UpdateService(int id, ServiceCatalogItem service)
    {
        if (id != service.Id)
        {
            return BadRequest();
        }

        var existing = await _context.Services.FirstOrDefaultAsync(item => item.Id == id);
        if (existing is null)
        {
            return NotFound();
        }

        existing.Name = service.Name;
        existing.Category = service.Category;
        existing.Provider = service.Provider;
        existing.Description = service.Description;
        existing.Eligibility = service.Eligibility;
        existing.AccessRules = service.AccessRules;
        existing.Owner = service.Owner;
        existing.IsApiAccess = service.IsApiAccess;
        existing.StudentTokenLimit = service.StudentTokenLimit;
        existing.EmployeeTokenLimit = service.EmployeeTokenLimit;

        await _context.SaveChangesAsync();
        return Ok(existing);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteService(int id)
    {
        var service = await _context.Services.FirstOrDefaultAsync(item => item.Id == id);
        if (service is null)
        {
            return NotFound();
        }

        _context.Services.Remove(service);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
