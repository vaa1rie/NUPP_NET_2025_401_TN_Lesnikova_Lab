using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Transport.Infrastructure.Models;

namespace Transport.Infrastructure
{
    public class TransportContext : IdentityDbContext<User>
    {
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Bus> Buses { get; set; }
        public DbSet<TechnicalPassport> TechnicalPassports { get; set; }
        public DbSet<MaintenanceRecord> MaintenanceRecords { get; set; }
        public DbSet<Route> Routes { get; set; }

        public TransportContext(DbContextOptions<TransportContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Vehicle>()
                .ToTable("Vehicles")
                .UseTptMappingStrategy();

            modelBuilder.Entity<Bus>()
                .ToTable("Buses");

            modelBuilder.Entity<Tram>()
                .ToTable("Trams");

            modelBuilder.Entity<Trolleybus>()
                .ToTable("Trolleybuses");


            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.TechnicalPassport)
                .WithOne(tp => tp.Vehicle)
                .HasForeignKey<TechnicalPassport>(tp => tp.VehicleId);


            modelBuilder.Entity<Vehicle>()
                .HasMany(v => v.MaintenanceRecords)
                .WithOne(mr => mr.Vehicle)
                .HasForeignKey(mr => mr.VehicleId);


            modelBuilder.Entity<RouteVehicle>()
                .HasKey(rv => new { rv.RouteId, rv.VehicleId });

            modelBuilder.Entity<RouteVehicle>()
                .HasOne(rv => rv.Route)
                .WithMany(r => r.RouteVehicles)
                .HasForeignKey(rv => rv.RouteId);

            modelBuilder.Entity<RouteVehicle>()
                .HasOne(rv => rv.Vehicle)
                .WithMany(v => v.RouteVehicles)
                .HasForeignKey(rv => rv.VehicleId);


            modelBuilder.Entity<Vehicle>()
                .Property(v => v.Model)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Route>()
                .Property(r => r.Number)
                .IsRequired()
                .HasMaxLength(10);
        }
    }
}