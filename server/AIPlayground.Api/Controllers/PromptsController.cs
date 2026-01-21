using AIPlayground.Api.Data;
using AIPlayground.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AIPlayground.Api.Controllers;

[ApiController]
[Route("api/prompts")]
public class PromptsController : ControllerBase
{
    private readonly AppDbContext _context;

    public PromptsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PromptLibraryItem>>> GetPrompts()
    {
        var prompts = await _context.Prompts.AsNoTracking().ToListAsync();
        return Ok(prompts);
    }
}
