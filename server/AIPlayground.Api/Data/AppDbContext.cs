using AIPlayground.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace AIPlayground.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<ServiceCatalogItem> Services => Set<ServiceCatalogItem>();
    public DbSet<PolicyDocument> Policies => Set<PolicyDocument>();
    public DbSet<AccessRequest> AccessRequests => Set<AccessRequest>();
    public DbSet<PromptLibraryItem> Prompts => Set<PromptLibraryItem>();
    public DbSet<RoleProfile> Roles => Set<RoleProfile>();
}
