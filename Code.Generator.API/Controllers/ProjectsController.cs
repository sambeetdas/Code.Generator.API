using Code.Generator.API.Models;
using Code.Generator.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Code.Generator.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly IGenerationService _generationService;

        public ProjectsController(IProjectService projectService, IGenerationService generationService)
        {
            _projectService = projectService;
            _generationService = generationService;
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

        [HttpGet]
        public async Task<IActionResult> GetProjects()
        {
            var projects = await _projectService.GetUserProjectsAsync(GetUserId());
            return Ok(projects);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProject(int id)
        {
            var project = await _projectService.GetProjectAsync(id, GetUserId());
            if (project == null)
                return NotFound();

            return Ok(project);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectDto dto)
        {
            var project = await _projectService.CreateProjectAsync(dto, GetUserId());
            return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, [FromBody] UpdateProjectDto dto)
        {
            var project = await _projectService.UpdateProjectAsync(id, dto, GetUserId());
            if (project == null)
                return NotFound();

            return Ok(project);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var success = await _projectService.DeleteProjectAsync(id, GetUserId());
            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpPost("{id}/generate-diagram")]
        public async Task<IActionResult> GenerateDiagram(int id)
        {
            var project = await _projectService.GetProjectAsync(id, GetUserId());
            if (project == null)
                return NotFound();

            try
            {
                var mermaidDiagram = await _generationService.GenerateMermaidDiagramAsync(project.Requirements);

                var updateDto = new UpdateProjectDto
                {
                    MermaidDiagram = mermaidDiagram,
                    Status = ProjectStatus.DesignGenerated
                };

                var updatedProject = await _projectService.UpdateProjectAsync(id, updateDto, GetUserId());
                return Ok(updatedProject);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("{id}/generate-code")]
        public async Task<IActionResult> GenerateCode(int id)
        {
            var project = await _projectService.GetProjectAsync(id, GetUserId());
            if (project == null)
                return NotFound();

            if (string.IsNullOrEmpty(project.MermaidDiagram))
                return BadRequest(new { Message = "Mermaid diagram must be generated first" });

            try
            {
                // Temporary : Backend and frontend tech are hardcoded for simplicity : will be replaced with user input in the future
                string backendTech = "ASP.NET Core";
                 string frontendTech = "Angular";
                // End of temporary hardcoded values

                var generatedCode = await _generationService.GenerateCodeAsync(project.MermaidDiagram, project.Requirements, backendTech, frontendTech);

                var updateDto = new UpdateProjectDto
                {
                    GeneratedCode = generatedCode,
                    Status = ProjectStatus.CodeGenerated
                };

                var updatedProject = await _projectService.UpdateProjectAsync(id, updateDto, GetUserId());
                return Ok(updatedProject);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
