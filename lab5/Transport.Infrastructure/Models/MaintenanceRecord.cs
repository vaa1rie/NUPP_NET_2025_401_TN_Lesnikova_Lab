using System;

namespace Transport.Infrastructure.Models
{
    public class MaintenanceRecord
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }


        public int VehicleId { get; set; }
    public Vehicle? Vehicle { get; set; }
    }
}