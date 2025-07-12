using Code.Generator.API.Models;
using Code.Generator.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Code.Generator.API.Services
{
    public class ProjectService : IProjectService
    {
        private readonly CodeGenDbContext _context;

        public ProjectService(CodeGenDbContext context)
        {
            _context = context;
        }

        public async Task<List<Project>> GetUserProjectsAsync(string userId)
        {
            return await _context.Projects
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.UpdatedAt)
                .ToListAsync();
        }

        public async Task<Project> GetProjectAsync(int id, string userId)
        {
            return await _context.Projects
                .Include(p => p.Deployments)
                .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);
        }

        public async Task<Project> CreateProjectAsync(CreateProjectDto dto, string userId)
        {
            var project = new Project
            {
                Name = dto.Name,
                Description = dto.Description,
                Requirements = dto.Requirements,
                UserId = userId
            };

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return project;
        }

        public async Task<Project> UpdateProjectAsync(int id, UpdateProjectDto dto, string userId)
        {
            var project = await GetProjectAsync(id, userId);
            if (project == null) return null;

            project.Name = dto.Name ?? project.Name;
            project.Description = dto.Description ?? project.Description;
            project.Requirements = dto.Requirements ?? project.Requirements;
            project.MermaidDiagram = dto.MermaidDiagram ?? project.MermaidDiagram;
            project.GeneratedCode = dto.GeneratedCode ?? project.GeneratedCode;
            project.Status = dto.Status ?? project.Status;
            project.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return project;
        }

        public async Task<bool> DeleteProjectAsync(int id, string userId)
        {
            var project = await GetProjectAsync(id, userId);
            if (project == null) return false;

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
