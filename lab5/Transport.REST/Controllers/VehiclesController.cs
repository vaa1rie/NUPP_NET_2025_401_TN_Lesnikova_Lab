using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Transport.Infrastructure;
using Transport.Infrastructure.Models;

namespace Transport.REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehiclesController : ControllerBase
    {
        private readonly TransportContext _context;
        public VehiclesController(TransportContext context)
        {
            _context = context;
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var vehicles = await _context.Vehicles.ToListAsync();
            return Ok(vehicles);
        }


        [HttpPost]
        [Authorize(Roles = "Admin,Dispatcher")]
        public async Task<IActionResult> Create([FromBody] Bus bus)
        {

            if (bus.TechnicalPassport != null)
            {
                bus.TechnicalPassport.Vehicle = bus;
            }
            if (bus.MaintenanceRecords != null)
            {
                foreach (var record in bus.MaintenanceRecords)
                {
                    record.Vehicle = bus;
                }
            }
            if (bus.RouteVehicles != null)
            {
                foreach (var rv in bus.RouteVehicles)
                {
                    rv.Vehicle = bus;
                }
            }
            _context.Buses.Add(bus);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAll), new { id = bus.Id }, bus);
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Dispatcher")]
        public async Task<IActionResult> Update(int id, [FromBody] Bus bus)
        {
            var existing = await _context.Buses.FindAsync(id);
            if (existing == null) return NotFound();
            existing.Model = bus.Model;
            existing.Year = bus.Year;
            existing.Seats = bus.Seats;

            await _context.SaveChangesAsync();
            return Ok(existing);
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null) return NotFound();
            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
