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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
