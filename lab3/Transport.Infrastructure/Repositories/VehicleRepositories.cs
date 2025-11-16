using Transport.Infrastructure.Models;

namespace Transport.Infrastructure.Repositories
{
    public class VehicleRepository<T> : BaseRepository<T> where T : Vehicle
    {
        public VehicleRepository(TransportContext context) : base(context)
        {
        }
    }

    public class BusRepository : VehicleRepository<Bus>
    {
        public BusRepository(TransportContext context) : base(context)
        {
        }
    }

    public class TramRepository : VehicleRepository<Tram>
    {
        public TramRepository(TransportContext context) : base(context)
        {
        }
    }

    public class TrolleybusRepository : VehicleRepository<Trolleybus>
    {
        public TrolleybusRepository(TransportContext context) : base(context)
        {
        }
    }
}