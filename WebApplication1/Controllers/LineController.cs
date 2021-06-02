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
        public async Task<IActionResult> Index(
            string sortOrder,
            int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["LineNameParm"] = String.IsNullOrEmpty(sortOrder) ? "lineName_desc" : "";

            var lines = from r in _context.Lines
                          select r;

            ViewBag.lines = new SelectList(lines, "ID", "Name");

            String selectedLine = Request.Query["selected_line"].ToString();

            if (!String.IsNullOrEmpty(selectedLine))
            {
                lines = lines.Where(s => s.ID.Equals(Convert.ToInt32(selectedLine)));
            }

            switch (sortOrder)
            {
                case "lineName_desc":
                    lines = lines.OrderByDescending(s => s.Name);
                    break;
                default:
                    lines = lines.OrderBy(s => s.Name);
                    break;
            }

            int pageSize = 3;
            //var line = _context.Lines.FromSqlRaw("EXECUTE dbo.Procedure").ToList();
            return View(await PaginatedList<LineModel>.CreateAsync(lines.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Line/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var lineModel = await _context.Lines
                .Include(s => s.Routes)
                    .ThenInclude(e => e.Buggy)
                .AsNoTracking()
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
        public async Task<IActionResult> Create([Bind("Name")] LineModel lineModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(lineModel);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
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
                        TempData["Message"] = "NotFound";
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["Message"] = "Edit succesful";
                return RedirectToAction(nameof(Index));
            }
            return View(lineModel);
        }

        // GET: Line/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lineModel = await _context.Lines
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (lineModel == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View(lineModel);
        }

        // POST: Line/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lineModel = await _context.Lines.FindAsync(id);
            if (lineModel == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Lines.Remove(lineModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool LineModelExists(int id)
        {
            return _context.Lines.Any(e => e.ID == id);
        }
    }
}
