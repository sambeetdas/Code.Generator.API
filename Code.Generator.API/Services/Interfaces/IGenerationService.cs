namespace Code.Generator.API.Services.Interfaces
{
    public interface IGenerationService
    {
        Task<string> GenerateMermaidDiagramAsync(string requirements);
        Task<string> GenerateCodeAsync(string mermaidDiagram, string requirements, string backendTech, string frontendTech);
    }
}
