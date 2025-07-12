using Code.Generator.API.Models;

namespace Code.Generator.API.Services.Interfaces
{
    public interface IDeploymentService
    {
        Task<Deployment> DeployAsync(int projectId, string environment, string userId);
        Task<List<Deployment>> GetDeploymentsAsync(int projectId, string userId);
    }
}
