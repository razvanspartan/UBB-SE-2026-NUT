using Microsoft.EntityFrameworkCore;
using TeamNut.Models;

namespace TeamNut.Repositories
{
    public class AppDbContext : DbContext
    {
        // These represent your tables in the database
        public DbSet<User> Users { get; set; }
        public DbSet<UserData> UserDatas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // This connects to the simple filename in DbConfig
            optionsBuilder.UseSqlite(DbConfig.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // This maps the relationship between User and UserData
            modelBuilder.Entity<UserData>()
                .HasOne<User>()
                .WithOne()
                .HasForeignKey<UserData>(ud => ud.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}