using ShaTask.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ShaTask.Controllers
{
    public class InvoiceDetailsController : Controller
    {
        private readonly ShaTaskContext _context;

        public InvoiceDetailsController(ShaTaskContext context)
        {
            _context = context;
        }

        // GET: InvoiceDetails
        public async Task<IActionResult> Index()
        {
            var shaTaskContext = _context.InvoiceDetails.Include(i => i.InvoiceHeader);
            return View(await shaTaskContext.ToListAsync());
        }

        // GET: InvoiceDetails/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.InvoiceDetails == null)
            {
                return NotFound();
            }

            var invoiceDetail = await _context.InvoiceDetails
                .Include(i => i.InvoiceHeader)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (invoiceDetail == null)
            {
                return NotFound();
            }

            return View(invoiceDetail);
        }

        // GET: InvoiceDetails/Create
        public IActionResult Create()
        {
            ViewData["InvoiceHeaderID"] = new SelectList(_context.InvoiceHeaders, "ID", "CustomerName");
            return View();
        }

        // POST: InvoiceDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,InvoiceHeaderID,ItemName,ItemCount,ItemPrice")] InvoiceDetail invoiceDetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(invoiceDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["InvoiceHeaderID"] = new SelectList(_context.InvoiceHeaders, "ID", "CustomerName", invoiceDetail.InvoiceHeaderID);
            return View(invoiceDetail);
        }

        // GET: InvoiceDetails/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.InvoiceDetails == null)
            {
                return NotFound();
            }

            var invoiceDetail = await _context.InvoiceDetails.FindAsync(id);
            if (invoiceDetail == null)
            {
                return NotFound();
            }
            ViewData["InvoiceHeaderID"] = new SelectList(_context.InvoiceHeaders, "ID", "CustomerName", invoiceDetail.InvoiceHeaderID);
            return View(invoiceDetail);
        }

        // POST: InvoiceDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ID,InvoiceHeaderID,ItemName,ItemCount,ItemPrice")] InvoiceDetail invoiceDetail)
        {
            if (id != invoiceDetail.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invoiceDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceDetailExists(invoiceDetail.ID))
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
            ViewData["InvoiceHeaderID"] = new SelectList(_context.InvoiceHeaders, "ID", "CustomerName", invoiceDetail.InvoiceHeaderID);
            return View(invoiceDetail);
        }

        // GET: InvoiceDetails/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.InvoiceDetails == null)
            {
                return NotFound();
            }

            var invoiceDetail = await _context.InvoiceDetails
                .Include(i => i.InvoiceHeader)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (invoiceDetail == null)
            {
                return NotFound();
            }

            return View(invoiceDetail);
        }

        // POST: InvoiceDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.InvoiceDetails == null)
            {
                return Problem("Entity set 'ShaTaskContext.InvoiceDetails'  is null.");
            }
            var invoiceDetail = await _context.InvoiceDetails.FindAsync(id);
            if (invoiceDetail != null)
            {
                _context.InvoiceDetails.Remove(invoiceDetail);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Edit", "Invoices", new { id = invoiceDetail.InvoiceHeaderID }); ;
        }

        private bool InvoiceDetailExists(long id)
        {
            return (_context.InvoiceDetails?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
