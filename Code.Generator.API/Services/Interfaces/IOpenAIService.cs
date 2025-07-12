namespace Code.Generator.API.Services.Interfaces
{
    public interface IOpenAIService
    {
        Task<string> GenerateMermaidDiagramAsync(string requirements);
        Task<string> GenerateCodeAsync(string mermaidDiagram, string requirements);
    }
}
