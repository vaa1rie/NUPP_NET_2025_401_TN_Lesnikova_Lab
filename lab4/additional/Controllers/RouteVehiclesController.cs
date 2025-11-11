using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Transport.Infrastructure;
using Transport.Infrastructure.Models;

namespace additional.Controllers
{
    public class RouteVehiclesController : Controller
    {
        private readonly TransportContext _context;

        public RouteVehiclesController(TransportContext context)
        {
            _context = context;
        }

        // GET: RouteVehicles
        public async Task<IActionResult> Index()
        {
            var transportContext = _context.RouteVehicles.Include(r => r.Route).Include(r => r.Vehicle);
            return View(await transportContext.ToListAsync());
        }

        // GET: RouteVehicles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var routeVehicle = await _context.RouteVehicles
                .Include(r => r.Route)
                .Include(r => r.Vehicle)
                .FirstOrDefaultAsync(m => m.RouteId == id);
            if (routeVehicle == null)
            {
                return NotFound();
            }

            return View(routeVehicle);
        }

        // GET: RouteVehicles/Create
        public IActionResult Create()
        {
            ViewData["RouteId"] = new SelectList(_context.Routes, "Id", "EndPoint");
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "Model");
            return View();
        }

        // POST: RouteVehicles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RouteId,VehicleId")] RouteVehicle routeVehicle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(routeVehicle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RouteId"] = new SelectList(_context.Routes, "Id", "EndPoint", routeVehicle.RouteId);
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "Model", routeVehicle.VehicleId);
            return View(routeVehicle);
        }

        // GET: RouteVehicles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var routeVehicle = await _context.RouteVehicles.FindAsync(id);
            if (routeVehicle == null)
            {
                return NotFound();
            }
            ViewData["RouteId"] = new SelectList(_context.Routes, "Id", "EndPoint", routeVehicle.RouteId);
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "Model", routeVehicle.VehicleId);
            return View(routeVehicle);
        }

        // POST: RouteVehicles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RouteId,VehicleId")] RouteVehicle routeVehicle)
        {
            if (id != routeVehicle.RouteId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(routeVehicle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RouteVehicleExists(routeVehicle.RouteId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RouteId"] = new SelectList(_context.Routes, "Id", "EndPoint", routeVehicle.RouteId);
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "Model", routeVehicle.VehicleId);
            return View(routeVehicle);
        }

        // GET: RouteVehicles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var routeVehicle = await _context.RouteVehicles
                .Include(r => r.Route)
                .Include(r => r.Vehicle)
                .FirstOrDefaultAsync(m => m.RouteId == id);
            if (routeVehicle == null)
            {
                return NotFound();
            }

            return View(routeVehicle);
        }

        // POST: RouteVehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var routeVehicle = await _context.RouteVehicles.FindAsync(id);
            if (routeVehicle != null)
            {
                _context.RouteVehicles.Remove(routeVehicle);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RouteVehicleExists(int id)
        {
            return _context.RouteVehicles.Any(e => e.RouteId == id);
        }
    }
}
