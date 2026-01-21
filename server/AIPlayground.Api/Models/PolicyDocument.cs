namespace AIPlayground.Api.Models;

public class PolicyDocument
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public DateTime PublishedOn { get; set; }
}
