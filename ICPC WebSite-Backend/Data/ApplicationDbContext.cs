using ICPC_WebSite_Backend.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ICPC_WebSite_Backend.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {


        }
        public DbSet<Community> communities { get; set; }
        public DbSet<CommunityMember> CommunityMember { get; set; }
        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);
            builder.Entity<Community>()
           .HasAlternateKey(c => c.Name)
           .HasName("AlternateKey_Name");


            builder.Entity<CommunityMember>().
                HasKey(cm => new { cm.MemberId, cm.CommunityId, cm.Role });
            builder.Entity<CommunityMember>()
                .HasOne(cm => cm.Member)
                .WithMany(b => b.CommunityRoles)
                .HasForeignKey(cm => cm.MemberId);
            builder.Entity<CommunityMember>()
                .HasOne(cm => cm.Community)
                .WithMany(c => c.CommunityMembers)
                .HasForeignKey(cm => cm.CommunityId);
        }
    }
}
