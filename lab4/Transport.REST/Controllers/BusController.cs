using Microsoft.AspNetCore.Mvc;
using Transport.REST.Models;
using Transport.Infrastructure.Models;
using Transport.Infrastructure.Services;

namespace Transport.REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BusController : ControllerBase
    {
        private readonly ICrudServiceAsync<Bus> _busService;

        public BusController(ICrudServiceAsync<Bus> busService)
        {
            _busService = busService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BusModel>>> GetAll()
        {
            var buses = await _busService.ReadAllAsync();
            return Ok(buses.Select(MapToModel));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BusModel>> Get(int id)
        {
            var bus = await _busService.ReadAsync(new Guid(id, 0, 0, new byte[8]));
            if (bus == null) return NotFound();
            return Ok(MapToModel(bus));
        }

        [HttpPost]
        public async Task<ActionResult<BusModel>> Create(BusModel model)
        {
            var bus = MapToEntity(model);
            var result = await _busService.CreateAsync(bus);
            if (!result) return BadRequest();
            return CreatedAtAction(nameof(Get), new { id = bus.Id }, MapToModel(bus));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, BusModel model)
        {
            var bus = MapToEntity(model);
            bus.Id = id;
            var result = await _busService.UpdateAsync(bus);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var bus = await _busService.ReadAsync(new Guid(id, 0, 0, new byte[8]));
            if (bus == null) return NotFound();
            var result = await _busService.RemoveAsync(bus);
            if (!result) return BadRequest();
            return NoContent();
        }

        // Мапінг між Bus і BusModel
        private static BusModel MapToModel(Bus bus) => new BusModel
        {
            Id = bus.Id,
            Model = bus.Model,
            Year = bus.Year,
            Seats = bus.Seats
        };

        private static Bus MapToEntity(BusModel model) => new Bus
        {
            Id = model.Id,
            Model = model.Model,
            Year = model.Year,
            Seats = model.Seats
        };
    }
}
