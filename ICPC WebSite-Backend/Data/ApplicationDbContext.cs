using ICPC_WebSite_Backend.Models;
using ICPC_WebSite_Backend.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ICPC_WebSite_Backend.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {


        }
        public DbSet<Community> communities { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Community>()
           .HasAlternateKey(c => c.Name)
           .HasName("AlternateKey_Name");
            var roles = new List<IdentityRole>();

            roles.Add(new IdentityRole
            {
                Id = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                Name = RolesList.CommunityLeader,
                NormalizedName = RolesList.CommunityLeader.ToUpper()
            });
            roles.Add(new IdentityRole
            {
                Id = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                Name = RolesList.HeadOfTraining,
                NormalizedName = RolesList.HeadOfTraining.ToUpper()
            });
            roles.Add(new IdentityRole
            {
                Id = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                Name = RolesList.TrainingManager,
                NormalizedName = RolesList.TrainingManager.ToUpper()
            });
            roles.Add(new IdentityRole
            {
                Id = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                Name = RolesList.Mentor,
                NormalizedName = RolesList.Mentor.ToUpper()
            });
            roles.Add(new IdentityRole
            {
                Id = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                Name = RolesList.Trainee,
                NormalizedName = RolesList.Trainee.ToUpper()
            });

            builder.Entity<IdentityRole>().HasData(
                            roles
                            );
        }
    }
}
