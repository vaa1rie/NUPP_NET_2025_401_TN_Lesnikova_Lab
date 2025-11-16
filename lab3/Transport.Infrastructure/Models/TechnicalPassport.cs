using System;

namespace Transport.Infrastructure.Models
{
    public class TechnicalPassport
    {
        public int Id { get; set; }
        public string SerialNumber { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }

        // One-to-One relationship with Vehicle
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
    }
}