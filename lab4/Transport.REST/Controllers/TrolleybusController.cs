using Microsoft.AspNetCore.Mvc;
using Transport.REST.Models;
using Transport.Infrastructure.Models;
using Transport.Infrastructure.Services;

namespace Transport.REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrolleybusController : ControllerBase
    {
        private readonly ICrudServiceAsync<Trolleybus> _trolleybusService;

        public TrolleybusController(ICrudServiceAsync<Trolleybus> trolleybusService)
        {
            _trolleybusService = trolleybusService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrolleybusModel>>> GetAll()
        {
            var trolleybuses = await _trolleybusService.ReadAllAsync();
            return Ok(trolleybuses.Select(MapToModel));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TrolleybusModel>> Get(int id)
        {
            var trolleybus = await _trolleybusService.ReadAsync(new Guid(id, 0, 0, new byte[8]));
            if (trolleybus == null) return NotFound();
            return Ok(MapToModel(trolleybus));
        }

        [HttpPost]
        public async Task<ActionResult<TrolleybusModel>> Create(TrolleybusModel model)
        {
            var trolleybus = MapToEntity(model);
            var result = await _trolleybusService.CreateAsync(trolleybus);
            if (!result) return BadRequest();
            return CreatedAtAction(nameof(Get), new { id = trolleybus.Id }, MapToModel(trolleybus));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TrolleybusModel model)
        {
            var trolleybus = MapToEntity(model);
            trolleybus.Id = id;
            var result = await _trolleybusService.UpdateAsync(trolleybus);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var trolleybus = await _trolleybusService.ReadAsync(new Guid(id, 0, 0, new byte[8]));
            if (trolleybus == null) return NotFound();
            var result = await _trolleybusService.RemoveAsync(trolleybus);
            if (!result) return BadRequest();
            return NoContent();
        }

        private static TrolleybusModel MapToModel(Trolleybus trolleybus) => new TrolleybusModel
        {
            Id = trolleybus.Id,
            Model = trolleybus.Model,
            Year = trolleybus.Year,
            PowerSupply = trolleybus.PowerSupply
        };

        private static Trolleybus MapToEntity(TrolleybusModel model) => new Trolleybus
        {
            Id = model.Id,
            Model = model.Model,
            Year = model.Year,
            PowerSupply = model.PowerSupply
        };
    }
}