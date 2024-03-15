using BookingApp.Server.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BookingApp.Server.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) 
        {
        }

        public virtual DbSet<Product> Product => Set<Product>();
        public virtual DbSet<User> User => Set<User>();
        public virtual DbSet<Comment> Comment => Set<Comment>();
        public virtual DbSet<Role> Role => Set<Role>();
        public virtual DbSet<Chat> Chat => Set<Chat>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(x => x.Email)
                .IsUnique();
            modelBuilder.Entity<Role>()
                .HasIndex(x => x.Name)
                .IsUnique();
            modelBuilder.Entity<Product>()
                .HasIndex(x => x.Slug)
                .IsUnique();
            base.OnModelCreating(modelBuilder);
        }
    }
}
