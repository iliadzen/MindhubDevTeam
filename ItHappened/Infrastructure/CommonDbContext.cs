using System;
using ItHappened.Domain;
using Microsoft.EntityFrameworkCore;

namespace ItHappened.Infrastructure
{
    public class CommonDbContext : DbContext
    {
        public CommonDbContext(DbContextOptions options) : base(options)
        {
        }
        
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<License>(builder =>
            {
                builder.ToTable("Licenses", "ItHappened");
                builder.Property<Guid>("Id");
            });
            
            modelBuilder.Entity<User>(builder =>
            {
                builder.ToTable("Users", "ItHappened");
                builder.HasOne(user => user.License).WithOne().OnDelete(DeleteBehavior.Cascade).HasForeignKey<User>("LicenseId").OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}