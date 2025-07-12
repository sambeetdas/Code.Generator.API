using Microsoft.AspNetCore.Identity;

namespace Code.Generator.API.Models
{
    public class ApplicationUser
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<Project> Projects { get; set; } = new();
    }

}
