﻿using AwesomeDevEvents.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace AwesomeDevEvents.API.Persistence
{
    public class DevEventsDbContext : DbContext
    {
        public DevEventsDbContext(DbContextOptions<DevEventsDbContext> options) : base(options)
        {
        }

        public DbSet<DevEvent> DevEvent { get; set; }
        public DbSet<DevEventSpeaker> DevEventSpeaker { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DevEvent>(e =>
            {
                e.HasKey(de => de.Id);

                e.Property(de => de.Title).IsRequired(false);

                e.Property(de => de.Description)
                .HasMaxLength(200)
                .HasColumnType("varchar(200)");

                e.Property(de => de.StartDate)
                .HasColumnName("Start_Date");

                e.Property(de => de.EndDate)
                .HasColumnName("End_Date");

                e.HasMany(de => de.Speakers)
                    .WithOne()
                    .HasForeignKey(de => de.DevEventId);
            });

            modelBuilder.Entity<DevEventSpeaker>(e =>
            {
                e.HasKey(des => des.Id);
            });
        }
    }
}
