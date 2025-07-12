using Code.Generator.API.Models;
using Code.Generator.API.Services.Interfaces;
using System.Text;
using System.Text.Json;

namespace Code.Generator.API.Services
{
    public class OpenAIService : IOpenAIService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public OpenAIService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<string> GenerateMermaidDiagramAsync(string requirements)
        {
            var prompt = $@"Based on the following requirements, generate a detailed Mermaid diagram that represents the system architecture and flow. Only return the Mermaid code without any explanation:

Requirements: {requirements}

Please create a comprehensive diagram that includes:
1. System components
2. Data flow
3. User interactions
4. API endpoints if applicable
5. Database entities if applicable

Return only the Mermaid diagram code starting with the diagram type (graph, flowchart, etc.).";

            return await CallOpenAIAsync(prompt);
        }

        public async Task<string> GenerateCodeAsync(string mermaidDiagram, string requirements)
        {
            var prompt = $@"Based on the following Mermaid diagram and requirements, generate production-ready code. Include all necessary files, components, and configurations:

Mermaid Diagram:
{mermaidDiagram}

Requirements:
{requirements}

Please generate:
1. Backend API code (ASP.NET Core)
2. Frontend code (Angular)
3. Database models
4. Configuration files
5. README with setup instructions

Organize the code in a clear folder structure and include comments explaining key functionality.";

            return await CallOpenAIAsync(prompt);
        }

        private async Task<string> CallOpenAIAsync(string prompt)
        {
            var apiKey = _configuration["OpenAI:ApiKey"];
            var apiEndpoint = _configuration["OpenAI:BaseUrl"];
            string deploymentName = "gpt-4o-mini";
            string apiVersion = "2024-08-01-preview";
            
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

            var requestBody = new
            {
                messages = new[]
                {
                    new { role = "user", content = prompt }
                },
                max_tokens = 10000,
                temperature = 0.7
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            var url = $"{apiEndpoint}/openai/deployments/{deploymentName}/chat/completions?api-version={apiVersion}";
            var response = await _httpClient.PostAsync(url, jsonContent);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<OpenAIResponse>(responseContent);
                return RemoveCodeFences(result.choices[0].message.content);
            }

            throw new Exception($"OpenAI API call failed: {response.StatusCode}");
        }

        private string RemoveCodeFences(string input)
        {
            if (input.StartsWith("```"))
            {
                int firstNewline = input.IndexOf('\n');
                int lastFence = input.LastIndexOf("```");

                if (firstNewline > 0 && lastFence > firstNewline)
                {
                    return input.Substring(firstNewline + 1, lastFence - firstNewline - 1).Trim();
                }
            }
            return input.Trim();
        }
    }
}
