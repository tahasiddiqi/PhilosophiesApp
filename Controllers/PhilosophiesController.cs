using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAppTS.Data;
using WebAppTS.Models;

namespace WebAppTS.Controllers
{
    public class PhilosophiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PhilosophiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Philosophies
        public async Task<IActionResult> Index()
        {
            return View(await _context.Philosophy.ToListAsync()) ;
                         
        }
        // GET: Philosophies/ShowSearchForm
        public async Task<IActionResult> ShowSearchForm()
        {
            return View();
        }
        // POST: Philosophies/ShowSearchResults
        public async Task<IActionResult> ShowSearchResults(String SearchPhrase)
        {
            return View("Index", await _context.Philosophy.Where(j => j.Concern.Contains(SearchPhrase)).ToListAsync());
        }

            // GET: Philosophies/Details/5
            public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Philosophy == null)
            {
                return NotFound();
            }

            var philosophy = await _context.Philosophy
                .FirstOrDefaultAsync(m => m.ID == id);
            if (philosophy == null)
            {
                return NotFound();
            }

            return View(philosophy);
        }

        // GET: Philosophies/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Philosophies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Concern,Answer")] Philosophy philosophy)
        {
            if (ModelState.IsValid)
            {
                _context.Add(philosophy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(philosophy);
        }

        // GET: Philosophies/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Philosophy == null)
            {
                return NotFound();
            }

            var philosophy = await _context.Philosophy.FindAsync(id);
            if (philosophy == null)
            {
                return NotFound();
            }
            return View(philosophy);
        }

        // POST: Philosophies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Concern,Answer")] Philosophy philosophy)
        {
            if (id != philosophy.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(philosophy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhilosophyExists(philosophy.ID))
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
            return View(philosophy);
        }

        // GET: Philosophies/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Philosophy == null)
            {
                return NotFound();
            }

            var philosophy = await _context.Philosophy
                .FirstOrDefaultAsync(m => m.ID == id);
            if (philosophy == null)
            {
                return NotFound();
            }

            return View(philosophy);
        }

        // POST: Philosophies/Delete/5
   
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Philosophy == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Philosophy'  is null.");
            }
            var philosophy = await _context.Philosophy.FindAsync(id);
            if (philosophy != null)
            {
                _context.Philosophy.Remove(philosophy);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PhilosophyExists(int id)
        {
          return (_context.Philosophy?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
