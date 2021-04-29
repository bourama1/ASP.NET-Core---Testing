using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class BuggyController : Controller
    {
        private readonly FactoryContext _context;

        public BuggyController(FactoryContext context)
        {
            _context = context;
        }

        // GET: Buggy
        public async Task<IActionResult> Index()
        {
            return View(await _context.Buggies.ToListAsync());
        }

        // GET: Buggy/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var buggyModel = await _context.Buggies
                .FirstOrDefaultAsync(m => m.BuggyID == id);
            if (buggyModel == null)
            {
                return NotFound();
            }

            return View(buggyModel);
        }

        // GET: Buggy/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Buggy/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BuggyID,Name")] BuggyModel buggyModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(buggyModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(buggyModel);
        }

        // GET: Buggy/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var buggyModel = await _context.Buggies.FindAsync(id);
            if (buggyModel == null)
            {
                return NotFound();
            }
            return View(buggyModel);
        }

        // POST: Buggy/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BuggyID,Name")] BuggyModel buggyModel)
        {
            if (id != buggyModel.BuggyID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(buggyModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BuggyModelExists(buggyModel.BuggyID))
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
            return View(buggyModel);
        }

        // GET: Buggy/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var buggyModel = await _context.Buggies
                .FirstOrDefaultAsync(m => m.BuggyID == id);
            if (buggyModel == null)
            {
                return NotFound();
            }

            return View(buggyModel);
        }

        // POST: Buggy/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var buggyModel = await _context.Buggies.FindAsync(id);
            _context.Buggies.Remove(buggyModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BuggyModelExists(int id)
        {
            return _context.Buggies.Any(e => e.BuggyID == id);
        }
    }
}
