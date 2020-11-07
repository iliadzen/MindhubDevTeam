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
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Scale> Scales { get; set; }
        public DbSet<Geotag> Geotags { get; set; }
        public DbSet<Photo> Photos { get; set; }

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
            
            //Customizations
            AddCustomizationModel<Comment>(modelBuilder, "Comments");
            AddCustomizationModel<Rating>(modelBuilder, "Ratings");
            AddCustomizationModel<Scale>(modelBuilder, "Scales");
            AddCustomizationModel<Geotag>(modelBuilder, "Geotags");
            AddCustomizationModel<Photo>(modelBuilder, "Photos");
        }

        private void AddCustomizationModel<T>(ModelBuilder modelBuilder, string tableName)
        where T : class, IEventCustomizationData
        {
            modelBuilder.Entity<T>(builder =>
            {
                builder.ToTable(tableName, "ItHappened");
                builder.HasOne<Event>().WithMany().HasForeignKey(entity => entity.EventId);
            });
        }
    }
}