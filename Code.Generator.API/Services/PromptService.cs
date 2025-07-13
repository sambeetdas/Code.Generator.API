using Code.Generator.API.Models;
using Code.Generator.API.Services.Interfaces;
using System.Reflection.Metadata.Ecma335;

namespace Code.Generator.API.Services
{
    public class PromptService : IPromptService
    {
        public async Task<Prompt> MermaidGeneration(string requirements)
        {
            return  new Prompt
            {
                UserPrompt = $@"Based on the following requirements, generate a detailed Mermaid diagram that represents the system architecture and flow. Only return the Mermaid code without any explanation:

                                Requirements: {requirements}

                                Please create a comprehensive diagram that includes:
                                1. System components
                                2. Data flow
                                3. User interactions
                                4. API endpoints if applicable
                                5. Database entities if applicable

                                Return only the Mermaid diagram code starting with the diagram type (graph, flowchart, etc.)."
            };
        }

        public async Task<Prompt> CodeGeneration(string mermaidDiagram, string requirements, string backendTech, string frontendTech)
        {
            return new Prompt
            {
                UserPrompt = $@"Based on the following Mermaid diagram and requirements, generate production-ready code. Include all necessary files, components, and configurations:

                                Mermaid Diagram:
                                {mermaidDiagram}

                                Requirements:
                                {requirements}

                                Please generate:
                                1. Backend API code ({backendTech})
                                2. Frontend code ({frontendTech})
                                3. Database models
                                4. Configuration files
                                5. README with setup instructions

                                Organize the code in a clear folder structure and include comments explaining key functionality."
            };
        }
    }
}
