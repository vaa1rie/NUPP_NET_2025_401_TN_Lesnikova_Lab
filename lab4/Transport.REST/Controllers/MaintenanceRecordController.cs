using Microsoft.AspNetCore.Mvc;
using Transport.REST.Models;

namespace Transport.REST.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Transport.REST.Models;
    using Transport.Infrastructure.Models;
    using Transport.Infrastructure.Services;

    [ApiController]
    [Route("api/[controller]")]
    public class MaintenanceRecordController : ControllerBase
    {
        private readonly ICrudServiceAsync<MaintenanceRecord> _service;

        public MaintenanceRecordController(ICrudServiceAsync<MaintenanceRecord> service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MaintenanceRecordModel>>> GetAll()
        {
            var records = await _service.ReadAllAsync();
            return Ok(records.Select(MapToModel));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MaintenanceRecordModel>> Get(int id)
        {
            var record = await _service.ReadAsync(new Guid(id, 0, 0, new byte[8]));
            if (record == null) return NotFound();
            return Ok(MapToModel(record));
        }

        [HttpPost]
        public async Task<ActionResult<MaintenanceRecordModel>> Create(MaintenanceRecordModel model)
        {
            var entity = MapToEntity(model);
            var result = await _service.CreateAsync(entity);
            if (!result) return BadRequest();
            return CreatedAtAction(nameof(Get), new { id = entity.Id }, MapToModel(entity));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, MaintenanceRecordModel model)
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
            var record = await _service.ReadAsync(new Guid(id, 0, 0, new byte[8]));
            if (record == null) return NotFound();
            var result = await _service.RemoveAsync(record);
            if (!result) return BadRequest();
            return NoContent();
        }

        private static MaintenanceRecordModel MapToModel(MaintenanceRecord entity) => new MaintenanceRecordModel
        {
            Id = entity.Id,
            Date = entity.Date,
            Description = entity.Description,
            VehicleId = entity.VehicleId
        };

        private static MaintenanceRecord MapToEntity(MaintenanceRecordModel model) => new MaintenanceRecord
        {
            Id = model.Id,
            Date = model.Date,
            Description = model.Description,
            VehicleId = model.VehicleId
        };
    }
}
// ...existing code...