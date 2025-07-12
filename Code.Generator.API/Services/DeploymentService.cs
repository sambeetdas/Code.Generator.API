using Code.Generator.API.Models;
using Code.Generator.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Code.Generator.API.Services
{
    public class DeploymentService : IDeploymentService
    {
        private readonly CodeGenDbContext _context;

        public DeploymentService(CodeGenDbContext context)
        {
            _context = context;
        }

        public async Task<Deployment> DeployAsync(int projectId, string environment, string userId)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId && p.UserId == userId);
            if (project == null) return null;

            var deployment = new Deployment
            {
                ProjectId = projectId,
                Environment = environment,
                Status = DeploymentStatus.Pending,
                Logs = "Deployment initiated..."
            };

            _context.Deployments.Add(deployment);
            await _context.SaveChangesAsync();

            // Simulate deployment process
            _ = Task.Run(async () => await ProcessDeploymentAsync(deployment.Id));

            return deployment;
        }

        public async Task<List<Deployment>> GetDeploymentsAsync(int projectId, string userId)
        {
            return await _context.Deployments
                .Where(d => d.ProjectId == projectId && d.Project.UserId == userId)
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync();
        }

        private async Task ProcessDeploymentAsync(int deploymentId)
        {
            var deployment = await _context.Deployments.FindAsync(deploymentId);
            if (deployment == null) return;

            try
            {
                deployment.Status = DeploymentStatus.InProgress;
                deployment.Logs += "\nBuilding application...";
                await _context.SaveChangesAsync();

                // Simulate build process
                await Task.Delay(5000);

                deployment.Status = DeploymentStatus.Success;
                deployment.DeploymentUrl = $"https://{deployment.Environment}-{deployment.ProjectId}.example.com";
                deployment.Logs += "\nDeployment completed successfully!";
            }
            catch (Exception ex)
            {
                deployment.Status = DeploymentStatus.Failed;
                deployment.Logs += $"\nDeployment failed: {ex.Message}";
            }

            await _context.SaveChangesAsync();
        }
    }
}
