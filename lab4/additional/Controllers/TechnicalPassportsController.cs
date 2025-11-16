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
    public class TechnicalPassportsController : Controller
    {
        private readonly TransportContext _context;

        public TechnicalPassportsController(TransportContext context)
        {
            _context = context;
        }

        // GET: TechnicalPassports
        public async Task<IActionResult> Index()
        {
            var transportContext = _context.TechnicalPassports.Include(t => t.Vehicle);
            return View(await transportContext.ToListAsync());
        }

        // GET: TechnicalPassports/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var technicalPassport = await _context.TechnicalPassports
                .Include(t => t.Vehicle)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (technicalPassport == null)
            {
                return NotFound();
            }

            return View(technicalPassport);
        }

        // GET: TechnicalPassports/Create
        public IActionResult Create()
        {
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "Model");
            return View();
        }

        // POST: TechnicalPassports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SerialNumber,IssueDate,ExpiryDate,VehicleId")] TechnicalPassport technicalPassport)
        {
            if (ModelState.IsValid)
            {
                _context.Add(technicalPassport);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "Model", technicalPassport.VehicleId);
            return View(technicalPassport);
        }

        // GET: TechnicalPassports/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var technicalPassport = await _context.TechnicalPassports.FindAsync(id);
            if (technicalPassport == null)
            {
                return NotFound();
            }
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "Model", technicalPassport.VehicleId);
            return View(technicalPassport);
        }

        // POST: TechnicalPassports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SerialNumber,IssueDate,ExpiryDate,VehicleId")] TechnicalPassport technicalPassport)
        {
            if (id != technicalPassport.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(technicalPassport);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TechnicalPassportExists(technicalPassport.Id))
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
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "Model", technicalPassport.VehicleId);
            return View(technicalPassport);
        }

        // GET: TechnicalPassports/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var technicalPassport = await _context.TechnicalPassports
                .Include(t => t.Vehicle)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (technicalPassport == null)
            {
                return NotFound();
            }

            return View(technicalPassport);
        }

        // POST: TechnicalPassports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var technicalPassport = await _context.TechnicalPassports.FindAsync(id);
            if (technicalPassport != null)
            {
                _context.TechnicalPassports.Remove(technicalPassport);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TechnicalPassportExists(int id)
        {
            return _context.TechnicalPassports.Any(e => e.Id == id);
        }
    }
}
