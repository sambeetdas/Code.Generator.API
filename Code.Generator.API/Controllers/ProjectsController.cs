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
        private readonly IOpenAIService _openAIService;

        public ProjectsController(IProjectService projectService, IOpenAIService openAIService)
        {
            _projectService = projectService;
            _openAIService = openAIService;
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
                var mermaidDiagram = await _openAIService.GenerateMermaidDiagramAsync(project.Requirements);

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
                var generatedCode = await _openAIService.GenerateCodeAsync(project.MermaidDiagram, project.Requirements);

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
