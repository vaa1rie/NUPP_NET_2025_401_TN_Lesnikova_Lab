using System;

namespace Transport.Infrastructure.Models
{
    public class Bus : Vehicle
    {
        public int Seats { get; set; }
        
        public static Bus CreateNew()
        {
            var rnd = new Random(Guid.NewGuid().GetHashCode());
            return new Bus
            {
                Seats = rnd.Next(20, 60),
                Year = rnd.Next(2000, 2025),
                Model = "Bus Model " + rnd.Next(1, 100)
            };
        }
    }
}