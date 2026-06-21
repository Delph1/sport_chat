using Microsoft.EntityFrameworkCore;
using Laktaren.Domain.Entities;

namespace Laktaren.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Reaction> Reactions { get; set; }
        public DbSet<Team> Teams { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<Post>()
                .HasOne(p => p.Author)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reaction>()
                .HasOne(r => r.Post)
                .WithMany(p => p.Reactions)
                .HasForeignKey(r => r.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reaction>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reactions)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reaction>()
                .HasIndex(r => new { r.UserId, r.PostId })
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasOne<Team>() 
                .WithMany()     
                .HasForeignKey(u => u.TeamId)
                .OnDelete(DeleteBehavior.SetNull); 

            modelBuilder.Entity<User>()
                .HasMany(u => u.SecondaryTeams)
                .WithMany(t => t.Supporters)
                .UsingEntity(j => j.ToTable("UserSecondaryTeams"));
        }
    }
}
