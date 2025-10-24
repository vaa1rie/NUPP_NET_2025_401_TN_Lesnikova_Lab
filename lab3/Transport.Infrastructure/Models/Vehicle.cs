using System;
using System.Collections.Generic;

namespace Transport.Infrastructure.Models
{
    public abstract class Vehicle
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }

        // One-to-One relationship
        public TechnicalPassport TechnicalPassport { get; set; }

        // One-to-Many relationship
        public ICollection<MaintenanceRecord> MaintenanceRecords { get; set; }

        // Many-to-Many relationship
        public ICollection<RouteVehicle> RouteVehicles { get; set; }

        public Vehicle()
        {
            MaintenanceRecords = new List<MaintenanceRecord>();
            RouteVehicles = new List<RouteVehicle>();
        }
    }
}