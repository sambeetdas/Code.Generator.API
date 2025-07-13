using Code.Generator.API.Models;
using Code.Generator.API.Services.Interfaces;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;
using System.Text.Json;

namespace Code.Generator.API.Services
{
    public class GenerationService : IGenerationService
    {
        private readonly IConfiguration _configuration;
        private readonly IPromptService _promptService;
        private readonly IOpenAIService _openAIService;

        public GenerationService(HttpClient httpClient, IConfiguration configuration, IPromptService promptService, IOpenAIService openAIService)
        {
            _configuration = configuration;
            _promptService = promptService;
            _openAIService = openAIService;
        }

        public async Task<string> GenerateMermaidDiagramAsync(string requirements)
        {
            var prompt = await _promptService.MermaidGeneration(requirements);

            return await _openAIService.CallOpenAIAsync(prompt);
        }

        public async Task<string> GenerateCodeAsync(string mermaidDiagram, string requirements, string backendTech, string frontendTech)
        {
            var prompt = await _promptService.CodeGeneration(mermaidDiagram, requirements, backendTech, frontendTech);

            return await _openAIService.CallOpenAIAsync(prompt);
        }
        
    }
}
