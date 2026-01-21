namespace AIPlayground.Api.Models;

public class PromptLibraryItem
{
    public int Id { get; set; }
    public string Track { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}
