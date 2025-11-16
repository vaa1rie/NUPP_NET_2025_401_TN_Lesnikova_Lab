using Microsoft.AspNetCore.Mvc;
using Transport.REST.Models;
using Transport.Infrastructure.Models;
using Transport.Infrastructure.Services;

namespace Transport.REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RouteVehicleController : ControllerBase
    {
        private readonly ICrudServiceAsync<RouteVehicle> _service;

        public RouteVehicleController(ICrudServiceAsync<RouteVehicle> service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RouteVehicleModel>>> GetAll()
        {
            var routeVehicles = await _service.ReadAllAsync();
            return Ok(routeVehicles.Select(MapToModel));
        }

        [HttpGet("{routeId}/{vehicleId}")]
        public async Task<ActionResult<RouteVehicleModel>> Get(int routeId, int vehicleId)
        {
            var all = await _service.ReadAllAsync();
            var rv = all.FirstOrDefault(x => x.RouteId == routeId && x.VehicleId == vehicleId);
            if (rv == null) return NotFound();
            return Ok(MapToModel(rv));
        }

        [HttpPost]
        public async Task<ActionResult<RouteVehicleModel>> Create(RouteVehicleModel model)
        {
            var entity = MapToEntity(model);
            var result = await _service.CreateAsync(entity);
            if (!result) return BadRequest();
            return CreatedAtAction(nameof(Get), new { routeId = entity.RouteId, vehicleId = entity.VehicleId }, MapToModel(entity));
        }

        [HttpDelete("{routeId}/{vehicleId}")]
        public async Task<IActionResult> Delete(int routeId, int vehicleId)
        {
            var all = await _service.ReadAllAsync();
            var rv = all.FirstOrDefault(x => x.RouteId == routeId && x.VehicleId == vehicleId);
            if (rv == null) return NotFound();
            var result = await _service.RemoveAsync(rv);
            if (!result) return BadRequest();
            return NoContent();
        }

        private static RouteVehicleModel MapToModel(RouteVehicle entity) => new RouteVehicleModel
        {
            RouteId = entity.RouteId,
            VehicleId = entity.VehicleId
        };

        private static RouteVehicle MapToEntity(RouteVehicleModel model) => new RouteVehicle
        {
            RouteId = model.RouteId,
            VehicleId = model.VehicleId
        };
    }
}