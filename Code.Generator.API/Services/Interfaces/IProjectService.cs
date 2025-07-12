using Code.Generator.API.Models;

namespace Code.Generator.API.Services.Interfaces
{
    public interface IProjectService
    {
        Task<List<Project>> GetUserProjectsAsync(string userId);
        Task<Project> GetProjectAsync(int id, string userId);
        Task<Project> CreateProjectAsync(CreateProjectDto dto, string userId);
        Task<Project> UpdateProjectAsync(int id, UpdateProjectDto dto, string userId);
        Task<bool> DeleteProjectAsync(int id, string userId);
    }
}
