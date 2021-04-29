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
    public class LineController : Controller
    {
        private readonly FactoryContext _context;

        public LineController(FactoryContext context)
        {
            _context = context;
        }

        // GET: Line
        public async Task<IActionResult> Index()
        {
            return View(await _context.Lines.ToListAsync());
        }

        // GET: Line/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lineModel = await _context.Lines
                .FirstOrDefaultAsync(m => m.ID == id);
            if (lineModel == null)
            {
                return NotFound();
            }

            return View(lineModel);
        }

        // GET: Line/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Line/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name")] LineModel lineModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lineModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lineModel);
        }

        // GET: Line/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lineModel = await _context.Lines.FindAsync(id);
            if (lineModel == null)
            {
                return NotFound();
            }
            return View(lineModel);
        }

        // POST: Line/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name")] LineModel lineModel)
        {
            if (id != lineModel.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lineModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LineModelExists(lineModel.ID))
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
            return View(lineModel);
        }

        // GET: Line/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lineModel = await _context.Lines
                .FirstOrDefaultAsync(m => m.ID == id);
            if (lineModel == null)
            {
                return NotFound();
            }

            return View(lineModel);
        }

        // POST: Line/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lineModel = await _context.Lines.FindAsync(id);
            _context.Lines.Remove(lineModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LineModelExists(int id)
        {
            return _context.Lines.Any(e => e.ID == id);
        }
    }
}
