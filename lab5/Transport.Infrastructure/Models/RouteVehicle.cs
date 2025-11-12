namespace Transport.Infrastructure.Models
{
    public class RouteVehicle
    {
        public int RouteId { get; set; }
        public Route Route { get; set; }

        public int VehicleId { get; set; }
    public Vehicle? Vehicle { get; set; }
    }
}