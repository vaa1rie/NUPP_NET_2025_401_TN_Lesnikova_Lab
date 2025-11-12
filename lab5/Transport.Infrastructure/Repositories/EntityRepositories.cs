using Transport.Infrastructure.Models;

namespace Transport.Infrastructure.Repositories
{
    public class RouteRepository : BaseRepository<Route>
    {
        public RouteRepository(TransportContext context) : base(context) { }
    }

    public class MaintenanceRecordRepository : BaseRepository<MaintenanceRecord>
    {
        public MaintenanceRecordRepository(TransportContext context) : base(context) { }
    }

    public class TechnicalPassportRepository : BaseRepository<TechnicalPassport>
    {
        public TechnicalPassportRepository(TransportContext context) : base(context) { }
    }

    public class RouteVehicleRepository : BaseRepository<RouteVehicle>
    {
        public RouteVehicleRepository(TransportContext context) : base(context) { }
    }
}
