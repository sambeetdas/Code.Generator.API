namespace Code.Generator.API.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Requirements { get; set; }
        public string? MermaidDiagram { get; set; }
        public string? GeneratedCode { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public ProjectStatus Status { get; set; } = ProjectStatus.Draft;
        public List<Deployment>? Deployments { get; set; } = new();
    }
}
