using Microsoft.EntityFrameworkCore;
using SemesterCore.Models;

namespace SemesterCore.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<StudySession> StudySessions { get; set; }
        public DbSet<SemesterInfos> SemesterInfos { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Prevent multiple cascade paths by specifying Restrict/NoAction on one or more relationships

            modelBuilder.Entity<Module>()
                .HasOne(m => m.User)
                .WithMany(u => u.Modules)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Restrict); // or DeleteBehavior.NoAction

            modelBuilder.Entity<StudySession>()
                .HasOne(s => s.User)
                .WithMany(u => u.StudySessions)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Restrict); // or NoAction

            modelBuilder.Entity<StudySession>()
                .HasOne(s => s.Module)
                .WithMany(m => m.StudySessions)
                .HasForeignKey(s => s.ModuleId)
                .OnDelete(DeleteBehavior.Cascade); // only one cascade allowed

            modelBuilder.Entity<User>()
                .HasOne(u => u.SemesterInfos)
                .WithOne(s => s.User)
                .HasForeignKey<SemesterInfos>(s => s.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
