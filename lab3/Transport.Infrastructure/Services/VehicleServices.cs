using Transport.Infrastructure.Models;
using Transport.Infrastructure.Repositories;

namespace Transport.Infrastructure.Services
{
    public class VehicleService : BaseCrudService<Vehicle>
    {
        public VehicleService(IRepository<Vehicle> repository) : base(repository)
        {
        }
    }

    public class BusService : BaseCrudService<Bus>
    {
        public BusService(IRepository<Bus> repository) : base(repository)
        {
        }
    }

    public class TramService : BaseCrudService<Tram>
    {
        public TramService(IRepository<Tram> repository) : base(repository)
        {
        }
    }

    public class TrolleybusService : BaseCrudService<Trolleybus>
    {
        public TrolleybusService(IRepository<Trolleybus> repository) : base(repository)
        {
        }
    }
}