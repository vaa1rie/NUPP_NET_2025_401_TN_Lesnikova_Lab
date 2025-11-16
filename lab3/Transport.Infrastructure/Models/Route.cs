using System.Collections.Generic;

namespace Transport.Infrastructure.Models
{
    public class Route
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string StartPoint { get; set; }
        public string EndPoint { get; set; }

        // Many-to-Many relationship with Vehicle
        public ICollection<RouteVehicle> RouteVehicles { get; set; }

        public Route()
        {
            RouteVehicles = new List<RouteVehicle>();
        }
    }
}