namespace Code.Generator.API.Models
{
    public class Deployment
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public string Environment { get; set; }
        public string DeploymentUrl { get; set; }
        public DeploymentStatus Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string Logs { get; set; }
    }
}
