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
    public class RouteController : Controller
    {
        private readonly FactoryContext _context;

        public RouteController(FactoryContext context)
        {
            _context = context;
        }

        // GET: Route
        public async Task<IActionResult> Index(string sortOrder)
        {
            ViewData["RouteIDParm"] = String.IsNullOrEmpty(sortOrder) ? "routeID_desc" : "";
            ViewData["LineIDParm"] = sortOrder == "lineID" ? "lineID_desc" : "lineID";
            ViewData["BuggyIDParm"] = sortOrder == "buggyID" ? "buggyID_desc" : "buggyID";
            var routes = from r in _context.Routes
                           select r;
            switch (sortOrder)
            {
                case "routeID_desc":
                    routes = routes.OrderByDescending(s => s.RouteID);
                    break;
                case "lineID":
                    routes = routes.OrderBy(s => s.LineID);
                    break;
                case "lineID_desc":
                    routes = routes.OrderByDescending(s => s.LineID);
                    break;
                case "buggyID":
                    routes = routes.OrderBy(s => s.BuggyID);
                    break;
                case "buggyID_desc":
                    routes = routes.OrderByDescending(s => s.BuggyID);
                    break;
                default:
                    routes = routes.OrderBy(s => s.RouteID);
                    break;
            }
            return View(await routes.AsNoTracking().ToListAsync());
        }

        // GET: Route/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var routeModel = await _context.Routes
                .Include(r => r.Buggy)
                .Include(r => r.Line)
                .FirstOrDefaultAsync(m => m.RouteID == id);
            if (routeModel == null)
            {
                return NotFound();
            }

            return View(routeModel);
        }

        // GET: Route/Create
        public IActionResult Create()
        {
            ViewData["BuggyID"] = new SelectList(_context.Buggies, "BuggyID", "BuggyID");
            ViewData["LineID"] = new SelectList(_context.Lines, "ID", "ID");
            return View();
        }

        // POST: Route/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RouteID,LineID,BuggyID")] RouteModel routeModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(routeModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BuggyID"] = new SelectList(_context.Buggies, "BuggyID", "BuggyID", routeModel.BuggyID);
            ViewData["LineID"] = new SelectList(_context.Lines, "ID", "ID", routeModel.LineID);
            return View(routeModel);
        }

        // GET: Route/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var routeModel = await _context.Routes.FindAsync(id);
            if (routeModel == null)
            {
                return NotFound();
            }
            ViewData["BuggyID"] = new SelectList(_context.Buggies, "BuggyID", "BuggyID", routeModel.BuggyID);
            ViewData["LineID"] = new SelectList(_context.Lines, "ID", "ID", routeModel.LineID);
            return View(routeModel);
        }

        // POST: Route/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RouteID,LineID,BuggyID")] RouteModel routeModel)
        {
            if (id != routeModel.RouteID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(routeModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RouteModelExists(routeModel.RouteID))
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
            ViewData["BuggyID"] = new SelectList(_context.Buggies, "BuggyID", "BuggyID", routeModel.BuggyID);
            ViewData["LineID"] = new SelectList(_context.Lines, "ID", "ID", routeModel.LineID);
            return View(routeModel);
        }

        // GET: Route/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var routeModel = await _context.Routes
                .Include(r => r.Buggy)
                .Include(r => r.Line)
                .FirstOrDefaultAsync(m => m.RouteID == id);
            if (routeModel == null)
            {
                return NotFound();
            }

            return View(routeModel);
        }

        // POST: Route/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var routeModel = await _context.Routes.FindAsync(id);
            _context.Routes.Remove(routeModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RouteModelExists(int id)
        {
            return _context.Routes.Any(e => e.RouteID == id);
        }
    }
}
