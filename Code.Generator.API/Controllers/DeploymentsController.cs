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
    public class DeploymentsController : ControllerBase
    {
        private readonly IDeploymentService _deploymentService;

        public DeploymentsController(IDeploymentService deploymentService)
        {
            _deploymentService = deploymentService;
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

        [HttpPost("projects/{projectId}/deploy")]
        public async Task<IActionResult> Deploy(int projectId, [FromBody] DeployRequest request)
        {
            var deployment = await _deploymentService.DeployAsync(projectId, request.Environment, GetUserId());
            if (deployment == null)
                return NotFound();

            return Ok(deployment);
        }

        [HttpGet("projects/{projectId}")]
        public async Task<IActionResult> GetDeployments(int projectId)
        {
            var deployments = await _deploymentService.GetDeploymentsAsync(projectId, GetUserId());
            return Ok(deployments);
        }
    }
}
