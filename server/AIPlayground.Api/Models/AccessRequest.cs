namespace AIPlayground.Api.Models;

public class AccessRequest
{
    public int Id { get; set; }
    public string RequesterName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public int ServiceId { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public string ApplicationName { get; set; } = string.Empty;
    public string IntendedUse { get; set; } = string.Empty;
    public string Duration { get; set; } = string.Empty;
    public int EstimatedTokens { get; set; }
    public string Status { get; set; } = "Pending";
    public string DecisionNotes { get; set; } = string.Empty;
    public DateTime SubmittedAt { get; set; }
}
