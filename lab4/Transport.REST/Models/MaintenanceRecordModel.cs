namespace Transport.REST.Models
{
    public class MaintenanceRecordModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }
        public int VehicleId { get; set; }
    }
}