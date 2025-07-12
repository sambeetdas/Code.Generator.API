using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Code.Generator.API.Models
{
    public class CodeGenDbContext : DbContext
    {
        public CodeGenDbContext(DbContextOptions<CodeGenDbContext> options) : base(options)
        { }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Deployment> Deployments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Project>()
                .HasOne(p => p.User)
                .WithMany(u => u.Projects)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Deployment>()
                .HasOne(d => d.Project)
                .WithMany(p => p.Deployments)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure string lengths to avoid SQL Server issues
            builder.Entity<Project>()
                .Property(p => p.Name)
                .HasMaxLength(200);

            builder.Entity<Project>()
                .Property(p => p.Description)
                .HasMaxLength(1000);

            builder.Entity<ApplicationUser>()
                .Property(u => u.FirstName)
                .HasMaxLength(100);

            builder.Entity<ApplicationUser>()
                .Property(u => u.LastName)
                .HasMaxLength(100);
        }
    }
}
