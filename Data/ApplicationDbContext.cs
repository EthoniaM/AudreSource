using AudreSource.Model;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AudreySource.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
       public DbSet<RoleInfo> Roles { get; set; }
       public DbSet<Organisation> Organisations { get; set; }
       public DbSet<User> Users { get; set; }
       public DbSet<UserDetail> UserDetails { get; set; }
        public DbSet<Audit> Audits{ get; set; }
        public DbSet<Kanban> Kanbans { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Organisation>().ToTable("Organisations");
            modelBuilder.Entity<RoleInfo>().ToTable("Roles");
            modelBuilder.Entity<User>()
                        .HasOne(u => u.UserDetails)
                        .WithOne(ud => ud.User)
                        .HasForeignKey<UserDetail>(ud => ud.UsersID);
            modelBuilder.Entity<Audit>()
                  .HasMany(a => a.Kanbans)
                  .WithOne(k => k.Audit)
                  .HasForeignKey(k => k.AuditID);
        }
    }
}

