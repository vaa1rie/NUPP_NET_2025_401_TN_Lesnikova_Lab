using Microsoft.AspNetCore.Mvc;
using Transport.REST.Models;
using Transport.Infrastructure.Models;
using Transport.Infrastructure.Services;

namespace Transport.REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RouteController : ControllerBase
    {
        private readonly ICrudServiceAsync<Transport.Infrastructure.Models.Route> _routeService;

        public RouteController(ICrudServiceAsync<Transport.Infrastructure.Models.Route> routeService)
        {
            _routeService = routeService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RouteModel>>> GetAll()
        {
            var routes = await _routeService.ReadAllAsync();
            return Ok(routes.Select(MapToModel));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RouteModel>> Get(int id)
        {
            var route = await _routeService.ReadAsync(new Guid(id, 0, 0, new byte[8]));
            if (route == null) return NotFound();
            return Ok(MapToModel(route));
        }

        [HttpPost]
        public async Task<ActionResult<RouteModel>> Create(RouteModel model)
        {
            var route = MapToEntity(model);
            var result = await _routeService.CreateAsync(route);
            if (!result) return BadRequest();
            return CreatedAtAction(nameof(Get), new { id = route.Id }, MapToModel(route));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, RouteModel model)
        {
            var route = MapToEntity(model);
            route.Id = id;
            var result = await _routeService.UpdateAsync(route);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var route = await _routeService.ReadAsync(new Guid(id, 0, 0, new byte[8]));
            if (route == null) return NotFound();
            var result = await _routeService.RemoveAsync(route);
            if (!result) return BadRequest();
            return NoContent();
        }

        private static RouteModel MapToModel(Transport.Infrastructure.Models.Route route) => new RouteModel
        {
            Id = route.Id,
            Number = route.Number,
            StartPoint = route.StartPoint,
            EndPoint = route.EndPoint
        };

        private static Transport.Infrastructure.Models.Route MapToEntity(RouteModel model) => new Transport.Infrastructure.Models.Route
        {
            Id = model.Id,
            Number = model.Number,
            StartPoint = model.StartPoint,
            EndPoint = model.EndPoint
        };
    }
}