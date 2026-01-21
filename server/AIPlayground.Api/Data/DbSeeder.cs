using AIPlayground.Api.Models;

namespace AIPlayground.Api.Data;

public static class DbSeeder
{
    public static void Seed(AppDbContext context)
    {
        if (context.Services.Any())
        {
            return;
        }

        context.Services.AddRange(
            new ServiceCatalogItem
            {
                Name = "Generative AI Platforms",
                Category = "AI Services",
                Provider = "OpenAI",
                Description = "Access to approved generative AI tools for training and research.",
                Eligibility = "Students, educators, and ITI employees with policy acknowledgment.",
                AccessRules = "Requests reviewed by approvers and service owners.",
                Owner = "AI Governance Office",
                IsApiAccess = true,
                StudentTokenLimit = 100000,
                EmployeeTokenLimit = 250000
            },
            new ServiceCatalogItem
            {
                Name = "Cloud AI Resources",
                Category = "Cloud Computing",
                Provider = "Azure",
                Description = "Provisioned Azure and AWS AI services with compliance guardrails.",
                Eligibility = "Service owners and approved project teams.",
                AccessRules = "Usage monitored with quarterly audits.",
                Owner = "Infrastructure Division",
                IsApiAccess = false,
                StudentTokenLimit = 0,
                EmployeeTokenLimit = 0
            });

        context.Policies.Add(new PolicyDocument
        {
            Title = "ITI Artificial Intelligence Policy",
            Version = "v1.0",
            Summary = "Defines responsible AI usage, human oversight, and data privacy safeguards.",
            PublishedOn = DateTime.UtcNow.AddDays(-45)
        });

        context.Roles.AddRange(
            new RoleProfile
            {
                Name = "Student / Intern",
                Responsibilities = "Browse approved services, submit requests, and follow AI policies."
            },
            new RoleProfile
            {
                Name = "Educator / Trainer",
                Responsibilities = "Develop prompts, review AI outputs, and mentor responsible usage."
            },
            new RoleProfile
            {
                Name = "Service Owner",
                Responsibilities = "Define services, eligibility criteria, and access instructions."
            },
            new RoleProfile
            {
                Name = "Policy Administrator",
                Responsibilities = "Manage AI policy publication, versioning, and enforcement."
            });

        context.AccessRequests.Add(new AccessRequest
        {
            RequesterName = "Nour Ahmed",
            Role = "Educator",
            ServiceId = 1,
            ServiceName = "Generative AI Platforms",
            ApplicationName = "AI Track Assessments",
            IntendedUse = "Create formative assessments for AI track.",
            Duration = "3 months",
            EstimatedTokens = 45000,
            Status = "Approved",
            DecisionNotes = "Approved with monthly usage reporting.",
            SubmittedAt = DateTime.UtcNow.AddDays(-10)
        });

        context.Prompts.AddRange(
            new PromptLibraryItem
            {
                Track = "AI Track",
                Title = "Gradient Descent Analogy",
                Description = "Explain gradient descent using a simple visual analogy with steps.",
                Status = "Published"
            },
            new PromptLibraryItem
            {
                Track = "Cybersecurity Track",
                Title = "Threat Modeling Review",
                Description = "Assess system architecture and propose mitigations.",
                Status = "In Review"
            });

        context.SaveChanges();
    }
}
