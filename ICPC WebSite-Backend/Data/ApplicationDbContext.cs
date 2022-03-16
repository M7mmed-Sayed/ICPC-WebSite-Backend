using ICPC_WebSite_Backend.Models;
using ICPC_WebSite_Backend.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ICPC_WebSite_Backend.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {


        }
        public DbSet<Community> communities { get; set; }
        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);
            builder.Entity<Community>()
           .HasAlternateKey(c => c.Name)
           .HasName("AlternateKey_Name");
            var roles = new List<IdentityRole>();
            foreach(var role in RolesList.UserRoles)
            {
                roles.Add(new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    Name = role,
                    NormalizedName = role.ToUpper()
                });
            }
            builder.Entity<IdentityRole>().HasData(
                            roles
                            ) ;
        }
    }
}
