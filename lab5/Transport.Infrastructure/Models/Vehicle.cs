using System;
using System.Collections.Generic;

namespace Transport.Infrastructure.Models
{
    public abstract class Vehicle : BaseEntity
    {
        public string Model { get; set; }
        public int Year { get; set; }

        
        public TechnicalPassport TechnicalPassport { get; set; }

        
        public ICollection<MaintenanceRecord> MaintenanceRecords { get; set; }

        
        public ICollection<RouteVehicle> RouteVehicles { get; set; }

        public Vehicle()
        {
            MaintenanceRecords = new List<MaintenanceRecord>();
            RouteVehicles = new List<RouteVehicle>();
        }
    }
}