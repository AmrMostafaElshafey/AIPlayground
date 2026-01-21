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

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PromptLibraryItem>> GetPrompt(int id)
    {
        var prompt = await _context.Prompts.AsNoTracking().FirstOrDefaultAsync(item => item.Id == id);
        if (prompt is null)
        {
            return NotFound();
        }

        return Ok(prompt);
    }

    [HttpPost]
    public async Task<ActionResult<PromptLibraryItem>> CreatePrompt(PromptLibraryItem prompt)
    {
        _context.Prompts.Add(prompt);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetPrompt), new { id = prompt.Id }, prompt);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<PromptLibraryItem>> UpdatePrompt(int id, PromptLibraryItem prompt)
    {
        if (id != prompt.Id)
        {
            return BadRequest();
        }

        var existing = await _context.Prompts.FirstOrDefaultAsync(item => item.Id == id);
        if (existing is null)
        {
            return NotFound();
        }

        existing.Track = prompt.Track;
        existing.Title = prompt.Title;
        existing.Description = prompt.Description;
        existing.Status = prompt.Status;

        await _context.SaveChangesAsync();
        return Ok(existing);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeletePrompt(int id)
    {
        var prompt = await _context.Prompts.FirstOrDefaultAsync(item => item.Id == id);
        if (prompt is null)
        {
            return NotFound();
        }

        _context.Prompts.Remove(prompt);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
