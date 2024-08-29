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
       public DbSet<KanBanDetail> KanBanDetails { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //OrganisTION
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Organisation>().ToTable("Organisations");
            //Roles
            modelBuilder.Entity<RoleInfo>().ToTable("Roles");
            //User
            modelBuilder.Entity<User>()
                        .HasOne(u => u.UserDetails)
                        .WithOne(ud => ud.User)
                        .HasForeignKey<UserDetail>(ud => ud.UsersID);
            //Audit
            modelBuilder.Entity<Audit>()
                  .HasMany(a => a.Kanbans)
                  .WithOne(k => k.Audit)
                  .HasForeignKey(k => k.AuditID);

            //Kanban
            modelBuilder.Entity<KanBanDetail>()
    .HasKey(kbd => kbd.KanBanDetailsID);

            // Configure foreign keys and relationships
            modelBuilder.Entity<KanBanDetail>()
                .HasOne(kbd => kbd.Kanban)
                .WithMany(k => k.KanBanDetails)
                .HasForeignKey(kbd => kbd.KanBanID);

            modelBuilder.Entity<KanBanDetail>()
                .HasOne(kbd => kbd.User)
                .WithMany(u => u.KanBanDetails)
                .HasForeignKey(kbd => kbd.UsersID);

        }
    }
}

