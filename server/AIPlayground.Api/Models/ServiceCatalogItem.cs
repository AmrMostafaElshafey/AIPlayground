namespace AIPlayground.Api.Models;

public class ServiceCatalogItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Provider { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Eligibility { get; set; } = string.Empty;
    public string AccessRules { get; set; } = string.Empty;
    public string Owner { get; set; } = string.Empty;
    public bool IsApiAccess { get; set; }
    public int StudentTokenLimit { get; set; }
    public int EmployeeTokenLimit { get; set; }
}
