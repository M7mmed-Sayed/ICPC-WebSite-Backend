﻿using ICPC_WebSite_Backend.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ICPC_WebSite_Backend.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {


        }
        public DbSet<Community> Communities { get; set; }
        public DbSet<CommunityMember> CommunityMember { get; set; }
        public DbSet<CommunityRequest> CommunityRequests { get; set; }
        public DbSet<Week> Weeks { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Training> Trainings { get; set; }
        public DbSet<WeekSheet> WeeksSheets { get; set; }
        public DbSet<WeekTraining> WeeksTrainings { get; set; }
        public DbSet<Sheet> Sheets { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);
            builder.Entity<Community>()
           .HasAlternateKey(c => c.Name)
           .HasName("AlternateKey_Name");
            builder.Entity<Material>().HasOne(b => b.Weeks)
                .WithMany(ba => ba.Materials)
                .HasForeignKey(bi => bi.WeekId);
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
            
            builder.Entity<WeekSheet>().
                HasKey(cm => new { cm.SheetId, cm.WeekId });
            builder.Entity<WeekSheet>()
                .HasOne(cm => cm.Sheet)
                .WithMany(c => c.WeekSheets)
                .HasForeignKey(cm => cm.SheetId);
            builder.Entity<WeekSheet>()
                .HasOne(cm => cm.Week)
                .WithMany(c => c.WeekSheets)
                .HasForeignKey(cm => cm.WeekId);
            
            builder.Entity<WeekTraining>().
                HasKey(cm => new { cm.TrainingId, cm.WeekId });
            builder.Entity<WeekTraining>()
                .HasOne(cm => cm.Training)
                .WithMany(c => c.WeekTraining)
                .HasForeignKey(cm => cm.TrainingId);
            builder.Entity<WeekTraining>()
                .HasOne(cm => cm.Week)
                .WithMany(c => c.WeekTraining)
                .HasForeignKey(cm => cm.WeekId);
            
            builder.Entity<Week>()
                .HasOne(cm => cm.Community)
                .WithMany(c => c.Weeks)
                .HasForeignKey(cm => cm.CommunityId);
            
            builder.Entity<Training>()
                .HasOne(cm => cm.Community)
                .WithMany(c => c.Trainings)
                .HasForeignKey(cm => cm.CommunityId);
            
            
                
            
            
            
        }
    }
}
