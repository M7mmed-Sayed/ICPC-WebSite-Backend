using ICPC_WebSite_Backend.Data.Models;
using ICPC_WebSite_Backend.Models;
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
        public DbSet<CommunityRequest> CommunityRequests { get; set; }
        public DbSet<Week> weeks { get; set; }
        public DbSet<Matirial> matirials { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);
            builder.Entity<Community>()
           .HasAlternateKey(c => c.Name)
           .HasName("AlternateKey_Name");
            builder.Entity<Matirial>().HasOne(b => b.weeks)
                .WithMany(ba => ba.matirials)
                .HasForeignKey(bi => bi.weekId);
            builder.Entity<Week>(entity => {
                entity.HasIndex(w => w.Name).IsUnique();
            });



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

            builder.Entity<CommunityRequest>().
                HasKey(cm => new { cm.MemberId, cm.CommunityId });
            builder.Entity<CommunityRequest>()
                .HasOne(cm => cm.Member)
                .WithMany(b => b.CommunityRequests)
                .HasForeignKey(cm => cm.MemberId);
            builder.Entity<CommunityRequest>()
                .HasOne(cm => cm.Community)
                .WithMany(c => c.CommunityRequests)
                .HasForeignKey(cm => cm.CommunityId);
        }
    }
}
