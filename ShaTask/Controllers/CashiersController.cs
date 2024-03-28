using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShaTask.Models;

namespace ShaTask.Controllers
{
    public class CashiersController : Controller
    {
        private readonly ShaTaskContext _context;

        public CashiersController(ShaTaskContext context)
        {
            _context = context;
        }

        // GET: Cashiers
        public async Task<IActionResult> Index()
        {
            var shaTaskContext = _context.Cashiers.Include(c => c.Branch);
            return View(await shaTaskContext.ToListAsync());
        }

        // GET: Cashiers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Cashiers == null)
            {
                return NotFound();
            }

            var cashier = await _context.Cashiers
                .Include(c => c.Branch)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (cashier == null)
            {
                return NotFound();
            }

            return View(cashier);
        }

        // GET: Cashiers/Create
        public IActionResult Create()
        {
            ViewData["BranchID"] = new SelectList(_context.Branches, "ID", "BranchName");
            return View();
        }

        // POST: Cashiers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,CashierName,BranchID")] Cashier cashier)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cashier);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BranchID"] = new SelectList(_context.Branches, "ID", "BranchName", cashier.BranchID);
            return View(cashier);
        }

        // GET: Cashiers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Cashiers == null)
            {
                return NotFound();
            }

            var cashier = await _context.Cashiers.FindAsync(id);
            if (cashier == null)
            {
                return NotFound();
            }
            ViewData["BranchID"] = new SelectList(_context.Branches, "ID", "BranchName", cashier.BranchID);
            return View(cashier);
        }

        // POST: Cashiers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,CashierName,BranchID")] Cashier cashier)
        {
            if (id != cashier.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cashier);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CashierExists(cashier.ID))
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
            ViewData["BranchID"] = new SelectList(_context.Branches, "ID", "BranchName", cashier.BranchID);
            return View(cashier);
        }

        // GET: Cashiers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Cashiers == null)
            {
                return NotFound();
            }

            var cashier = await _context.Cashiers
                .Include(c => c.Branch)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (cashier == null)
            {
                return NotFound();
            }

            return View(cashier);
        }

        // POST: Cashiers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Cashiers == null)
            {
                return Problem("Entity set 'ShaTaskContext.Cashiers'  is null.");
            }
            var cashier = await _context.Cashiers.FindAsync(id);
            if (cashier != null)
            {
                _context.Cashiers.Remove(cashier);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CashierExists(int id)
        {
            return (_context.Cashiers?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
