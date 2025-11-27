using Microsoft.AspNetCore.Mvc;
using Transport.REST.Models;
using Transport.Infrastructure.Models;
using Transport.Infrastructure.Services;

namespace Transport.REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleController : ControllerBase
    {
        private readonly ICrudServiceAsync<Vehicle> _vehicleService;

        public VehicleController(ICrudServiceAsync<Vehicle> vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehicleModel>>> GetAll()
        {
            var vehicles = await _vehicleService.ReadAllAsync();
            return Ok(vehicles.Select(MapToModel));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VehicleModel>> Get(int id)
        {
            var vehicle = await _vehicleService.ReadAsync(new Guid(id, 0, 0, new byte[8]));
            if (vehicle == null) return NotFound();
            return Ok(MapToModel(vehicle));
        }

        [HttpPost]
        public async Task<ActionResult<VehicleModel>> Create(VehicleModel model)
        {
            var vehicle = MapToEntity(model);
            var result = await _vehicleService.CreateAsync(vehicle);
            if (!result) return BadRequest();
            return CreatedAtAction(nameof(Get), new { id = vehicle.Id }, MapToModel(vehicle));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, VehicleModel model)
        {
            var vehicle = MapToEntity(model);
            vehicle.Id = id;
            var result = await _vehicleService.UpdateAsync(vehicle);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var vehicle = await _vehicleService.ReadAsync(new Guid(id, 0, 0, new byte[8]));
            if (vehicle == null) return NotFound();
            var result = await _vehicleService.RemoveAsync(vehicle);
            if (!result) return BadRequest();
            return NoContent();
        }

        private static VehicleModel MapToModel(Vehicle vehicle) => new VehicleModel
        {
            Id = vehicle.Id,
            Model = vehicle.Model,
            Year = vehicle.Year
        };

        private static Vehicle MapToEntity(VehicleModel model) => new VehicleImpl
        {
            Id = model.Id,
            Model = model.Model,
            Year = model.Year
        };

        // Dummy implementation for abstract Vehicle
        private class VehicleImpl : Vehicle { }
    }
}