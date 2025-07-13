using Code.Generator.API.Models;

namespace Code.Generator.API.Services.Interfaces
{
    public interface IPromptService
    {
        Task<Prompt> MermaidGeneration(string requirements);
        Task<Prompt> CodeGeneration(string mermaidDiagram, string requirements, string backendTech, string frontendTech);
    }
}
