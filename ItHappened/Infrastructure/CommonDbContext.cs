using System;
using System.Collections.Generic;
using ItHappened.Application;
using ItHappened.Domain;
using ItHappened.Domain.Customizations;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ItHappened.Infrastructure
{
    public class CommonDbContext : DbContext
    {
        public CommonDbContext(DbContextOptions options) : base(options)
        {
        }
        
        public DbSet<User> Users { get; set; }
        public DbSet<License> Licenses { get; set; }
        public DbSet<Tracker> Trackers { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<License>(builder =>
            {
                builder.ToTable("Licenses", "ItHappened");
            });

            modelBuilder.Entity<User>(builder =>
            {
                builder.ToTable("Users", "ItHappened");
            });
            
            modelBuilder.Entity<Tracker>
            (builder =>
            {
                builder.ToTable("Trackers", "ItHappened");
                builder.HasOne<Tracker>().WithMany().HasForeignKey(tracker => tracker.UserId);
                builder.Property((tracker => tracker.Customizations))
                    .HasColumnName("Customizations")
                    .HasConversion(
                        c => JsonConvert.SerializeObject(c), 
                        c => JsonConvert.DeserializeObject<List<CustomizationType>>(c));
            });
            
            modelBuilder.Entity<Event>(builder =>
            {
                builder.ToTable("Events", "ItHappened");
                builder.HasOne<Event>().WithMany().HasForeignKey(@event => @event.TrackerId);
            });
            
            modelBuilder.Entity<Comment>(builder =>
            {
                builder.ToTable("Comments", "ItHappened");
                builder.HasOne<Event>().WithMany().HasForeignKey(comment => comment.EventId);
            });
        }
    }
}