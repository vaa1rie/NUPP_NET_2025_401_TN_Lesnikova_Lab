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
    public class TramsController : Controller
    {
        private readonly TransportContext _context;

        public TramsController(TransportContext context)
        {
            _context = context;
        }

        // GET: Trams
        public async Task<IActionResult> Index()
        {
            return View(await _context.Trams.ToListAsync());
        }

        // GET: Trams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tram = await _context.Trams
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tram == null)
            {
                return NotFound();
            }

            return View(tram);
        }

        // GET: Trams/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Trams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PowerSupply,Model,Year,Id")] Tram tram)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tram);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tram);
        }

        // GET: Trams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tram = await _context.Trams.FindAsync(id);
            if (tram == null)
            {
                return NotFound();
            }
            return View(tram);
        }

        // POST: Trams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PowerSupply,Model,Year,Id")] Tram tram)
        {
            if (id != tram.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tram);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TramExists(tram.Id))
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
            return View(tram);
        }

        // GET: Trams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tram = await _context.Trams
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tram == null)
            {
                return NotFound();
            }

            return View(tram);
        }

        // POST: Trams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tram = await _context.Trams.FindAsync(id);
            if (tram != null)
            {
                _context.Trams.Remove(tram);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TramExists(int id)
        {
            return _context.Trams.Any(e => e.Id == id);
        }
    }
}
