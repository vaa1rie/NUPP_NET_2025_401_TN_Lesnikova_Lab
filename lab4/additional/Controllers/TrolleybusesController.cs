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
    public class TrolleybusesController : Controller
    {
        private readonly TransportContext _context;

        public TrolleybusesController(TransportContext context)
        {
            _context = context;
        }

        // GET: Trolleybuses
        public async Task<IActionResult> Index()
        {
            return View(await _context.Trolleybuses.ToListAsync());
        }

        // GET: Trolleybuses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trolleybus = await _context.Trolleybuses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trolleybus == null)
            {
                return NotFound();
            }

            return View(trolleybus);
        }

        // GET: Trolleybuses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Trolleybuses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PowerSupply,Model,Year,Id")] Trolleybus trolleybus)
        {
            if (ModelState.IsValid)
            {
                _context.Add(trolleybus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(trolleybus);
        }

        // GET: Trolleybuses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trolleybus = await _context.Trolleybuses.FindAsync(id);
            if (trolleybus == null)
            {
                return NotFound();
            }
            return View(trolleybus);
        }

        // POST: Trolleybuses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PowerSupply,Model,Year,Id")] Trolleybus trolleybus)
        {
            if (id != trolleybus.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trolleybus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrolleybusExists(trolleybus.Id))
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
            return View(trolleybus);
        }

        // GET: Trolleybuses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trolleybus = await _context.Trolleybuses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trolleybus == null)
            {
                return NotFound();
            }

            return View(trolleybus);
        }

        // POST: Trolleybuses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trolleybus = await _context.Trolleybuses.FindAsync(id);
            if (trolleybus != null)
            {
                _context.Trolleybuses.Remove(trolleybus);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrolleybusExists(int id)
        {
            return _context.Trolleybuses.Any(e => e.Id == id);
        }
    }
}
