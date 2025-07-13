namespace Code.Generator.API.Models
{
    public class UpdateProjectDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Requirements { get; set; }
        public string? MermaidDiagram { get; set; }
        public string? GeneratedCode { get; set; }
        public ProjectStatus? Status { get; set; }
    }
}
