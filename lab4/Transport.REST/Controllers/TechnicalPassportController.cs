using Microsoft.AspNetCore.Mvc;
using Transport.REST.Models;
using Transport.Infrastructure.Models;
using Transport.Infrastructure.Services;

namespace Transport.REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TechnicalPassportController : ControllerBase
    {
        private readonly ICrudServiceAsync<TechnicalPassport> _service;

        public TechnicalPassportController(ICrudServiceAsync<TechnicalPassport> service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TechnicalPassportModel>>> GetAll()
        {
            var passports = await _service.ReadAllAsync();
            return Ok(passports.Select(MapToModel));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TechnicalPassportModel>> Get(int id)
        {
            var passport = await _service.ReadAsync(new Guid(id, 0, 0, new byte[8]));
            if (passport == null) return NotFound();
            return Ok(MapToModel(passport));
        }

        [HttpPost]
        public async Task<ActionResult<TechnicalPassportModel>> Create(TechnicalPassportModel model)
        {
            var entity = MapToEntity(model);
            var result = await _service.CreateAsync(entity);
            if (!result) return BadRequest();
            return CreatedAtAction(nameof(Get), new { id = entity.Id }, MapToModel(entity));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TechnicalPassportModel model)
        {
            var entity = MapToEntity(model);
            entity.Id = id;
            var result = await _service.UpdateAsync(entity);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var passport = await _service.ReadAsync(new Guid(id, 0, 0, new byte[8]));
            if (passport == null) return NotFound();
            var result = await _service.RemoveAsync(passport);
            if (!result) return BadRequest();
            return NoContent();
        }

        private static TechnicalPassportModel MapToModel(TechnicalPassport entity) => new TechnicalPassportModel
        {
            Id = entity.Id,
            SerialNumber = entity.SerialNumber,
            VehicleId = entity.VehicleId
        };

        private static TechnicalPassport MapToEntity(TechnicalPassportModel model) => new TechnicalPassport
        {
            Id = model.Id,
            SerialNumber = model.SerialNumber,
            VehicleId = model.VehicleId
        };
    }
}