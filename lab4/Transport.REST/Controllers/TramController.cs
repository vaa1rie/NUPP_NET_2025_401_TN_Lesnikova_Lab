using Microsoft.AspNetCore.Mvc;
using Transport.REST.Models;
using Transport.Infrastructure.Models;
using Transport.Infrastructure.Services;

namespace Transport.REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TramController : ControllerBase
    {
        private readonly ICrudServiceAsync<Tram> _tramService;

        public TramController(ICrudServiceAsync<Tram> tramService)
        {
            _tramService = tramService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TramModel>>> GetAll()
        {
            var trams = await _tramService.ReadAllAsync();
            return Ok(trams.Select(MapToModel));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TramModel>> Get(int id)
        {
            var tram = await _tramService.ReadAsync(new Guid(id, 0, 0, new byte[8]));
            if (tram == null) return NotFound();
            return Ok(MapToModel(tram));
        }

        [HttpPost]
        public async Task<ActionResult<TramModel>> Create(TramModel model)
        {
            var tram = MapToEntity(model);
            var result = await _tramService.CreateAsync(tram);
            if (!result) return BadRequest();
            return CreatedAtAction(nameof(Get), new { id = tram.Id }, MapToModel(tram));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TramModel model)
        {
            var tram = MapToEntity(model);
            tram.Id = id;
            var result = await _tramService.UpdateAsync(tram);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var tram = await _tramService.ReadAsync(new Guid(id, 0, 0, new byte[8]));
            if (tram == null) return NotFound();
            var result = await _tramService.RemoveAsync(tram);
            if (!result) return BadRequest();
            return NoContent();
        }

        private static TramModel MapToModel(Tram tram) => new TramModel
        {
            Id = tram.Id,
            Model = tram.Model,
            Year = tram.Year,
            PowerSupply = tram.PowerSupply
        };

        private static Tram MapToEntity(TramModel model) => new Tram
        {
            Id = model.Id,
            Model = model.Model,
            Year = model.Year,
            PowerSupply = model.PowerSupply
        };
    }
}