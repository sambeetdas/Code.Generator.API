using Code.Generator.API.Models;

namespace Code.Generator.API.Services.Interfaces
{
    public interface IOpenAIService
    {
        Task<string> CallOpenAIAsync(Prompt prompt);
    }
}
