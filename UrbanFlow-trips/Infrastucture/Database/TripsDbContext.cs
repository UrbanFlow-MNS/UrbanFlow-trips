using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using UrbanFlow_trips.Models;

namespace UrbanFlow_trips.Database;

public class TripsDbContext : DbContext
{
    public TripsDbContext(DbContextOptions<TripsDbContext> options) : base(options)
    {
    }
    
        public DbSet<Agency> Agencies { get; set; }
        public DbSet<Routes> Routes { get; set; }
        public DbSet<RouteType> RouteTypes { get; set; }
        public DbSet<Calendar> Calendars { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Stop_Times> Stops { get; set; }
        public DbSet<Stop_Trip> StopTrips { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Agency>()
                .HasKey(a => a.AgencyId);

            modelBuilder.Entity<Agency>()
                .HasMany(a => a.Routes)
                .WithOne(r => r.Agency)
                .HasForeignKey(r => r.AgencyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RouteType>()
                .HasKey(rt => rt.RouteTypeId);

            modelBuilder.Entity<RouteType>()
                .HasMany(rt => rt.Routes)
                .WithOne(r => r.RouteType)
                .HasForeignKey(r => r.RouteTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Routes>()
                .HasKey(r => r.RouteId);

            modelBuilder.Entity<Routes>()
                .HasMany(r => r.Trips)
                .WithOne(t => t.Routes)
                .HasForeignKey(t => t.RouteId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Calendar>()
                .HasKey(c => c.ServiceId);

            modelBuilder.Entity<Calendar>()
                .HasMany(c => c.Trips)
                .WithOne(t => t.Calendar)
                .HasForeignKey(t => t.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Stop_Times>()
                .HasKey(s => s.StopId);

            modelBuilder.Entity<Stop_Times>()
                .HasMany(s => s.StopTrips)
                .WithOne(st => st.Stop)
                .HasForeignKey(st => st.StopId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Trip>()
                .HasKey(t => t.TripId);

            modelBuilder.Entity<Trip>()
                .HasMany(t => t.StopTrips)
                .WithOne(st => st.Trip)
                .HasForeignKey(st => st.TripId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Stop_Trip>()
                .HasKey(t => new { t.TripId, t.StopId });

            modelBuilder.Entity<Stop_Trip>()
                .HasIndex(st => new { st.TripId, st.StopId })
                .IsUnique();
            
            modelBuilder.Entity<Stop_Times>()
                .HasOne(s => s.Agency)
                .WithMany(a => a.Stops)
                .HasForeignKey(s => s.AgencyId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
}