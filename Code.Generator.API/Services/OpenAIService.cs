using Code.Generator.API.Models;
using Code.Generator.API.Services.Interfaces;
using System.Net.Http;
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
        public async Task<string> CallOpenAIAsync(Prompt prompt)
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
                    new { role = "user", content = prompt.UserPrompt }
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
