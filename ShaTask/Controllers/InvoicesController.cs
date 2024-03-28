using ShaTask.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ShaTask.Controllers
{
    public class InvoicesController : Controller
    {
        private readonly ShaTaskContext _context;

        public InvoicesController(ShaTaskContext context)
        {
            _context = context;
        }

        // GET: Invoices
        public async Task<IActionResult> Index()
        {
            var shaTaskContext = _context.InvoiceHeaders.Include(i => i.Branch).Include(i => i.Cashier).Include(i => i.InvoiceDetails);
            return View(await shaTaskContext.ToListAsync());
        }

        // GET: Invoices/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.InvoiceHeaders == null)
            {
                return NotFound();
            }

            var invoiceHeader = await _context.InvoiceHeaders
                .Include(i => i.Branch)
                .Include(i => i.Cashier)
                .Include(i => i.InvoiceDetails)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (invoiceHeader == null)
            {
                return NotFound();
            }

            return View(invoiceHeader);
        }

        // GET: Invoices/Create
        public IActionResult Create()
        {
            ViewData["BranchID"] = new SelectList(_context.Branches, "ID", "BranchName");
            ViewData["CashierID"] = new SelectList(_context.Cashiers, "ID", "CashierName");
            return View();
        }

        // POST: Invoices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,CustomerName,Invoicedate,CashierID,BranchID,InvoiceDetails")] InvoiceHeader invoiceHeader)
        {
            if (ModelState.IsValid)
            {
                _context.Add(invoiceHeader);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BranchID"] = new SelectList(_context.Branches, "ID", "BranchName", invoiceHeader.BranchID);
            ViewData["CashierID"] = new SelectList(_context.Cashiers, "ID", "CashierName", invoiceHeader.CashierID);
            return View(invoiceHeader);
        }

        // GET: Invoices/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.InvoiceHeaders == null)
            {
                return NotFound();
            }

            var invoiceHeader = await _context.InvoiceHeaders
                .Include(i => i.InvoiceDetails)
                .FirstOrDefaultAsync(i => i.ID == id);
            if (invoiceHeader == null)
            {
                return NotFound();
            }
            ViewData["BranchID"] = new SelectList(_context.Branches, "ID", "BranchName", invoiceHeader.BranchID);
            ViewData["CashierID"] = new SelectList(_context.Cashiers, "ID", "CashierName", invoiceHeader.CashierID);
            return View(invoiceHeader);
        }

        // POST: Invoices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ID,CustomerName,Invoicedate,CashierID,BranchID,InvoiceDetails")] InvoiceHeader invoiceHeader)
        {
            if (id != invoiceHeader.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invoiceHeader);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceHeaderExists(invoiceHeader.ID))
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
            ViewData["BranchID"] = new SelectList(_context.Branches, "ID", "BranchName", invoiceHeader.BranchID);
            ViewData["CashierID"] = new SelectList(_context.Cashiers, "ID", "CashierName", invoiceHeader.CashierID);
            return View(invoiceHeader);
        }

        // GET: Invoices/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.InvoiceHeaders == null)
            {
                return NotFound();
            }

            var invoiceHeader = await _context.InvoiceHeaders
                .Include(i => i.Branch)
                .Include(i => i.Cashier)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (invoiceHeader == null)
            {
                return NotFound();
            }

            return View(invoiceHeader);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.InvoiceHeaders == null)
            {
                return Problem("Entity set 'ShaTaskContext.InvoiceHeaders'  is null.");
            }
            var invoiceHeader = await _context.InvoiceHeaders.FindAsync(id);
            var invoiceDetails = _context.InvoiceDetails.Where(i => i.InvoiceHeaderID == id).ToList();
            if (invoiceHeader != null)
            {
                _context.InvoiceDetails.RemoveRange(invoiceDetails);
                _context.InvoiceHeaders.Remove(invoiceHeader);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvoiceHeaderExists(long id)
        {
            return (_context.InvoiceHeaders?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
