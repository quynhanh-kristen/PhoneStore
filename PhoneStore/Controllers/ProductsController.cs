using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PhoneStore.Models;

namespace PhoneStore.Controllers
{
    public class ProductsController : Controller
    {
        private readonly PhoneManagementContext _context;

        public ProductsController(PhoneManagementContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index(string SearchString)
        {
            //var phoneManagementContext = _context.TblProduct.Include(t => t.IdCtgPhoneNavigation).Include(t => t.UserCreated);
            //return View(await phoneManagementContext.ToListAsync());

            var products = from m in _context.TblProduct
                           select m;

            if (!String.IsNullOrEmpty(SearchString))
            {
                products = products.Where(s => s.Name.IndexOf(SearchString) >= 0);
            }

            return View(await products.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblProduct = await _context.TblProduct
                .Include(t => t.IdCtgPhoneNavigation)
                .Include(t => t.UserCreated)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblProduct == null)
            {
                return NotFound();
            }

            return View(tblProduct);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["IdCtgPhone"] = new SelectList(_context.TblCategory, "Id", "Id");
            ViewData["UserCreatedId"] = new SelectList(_context.TblUser, "Id", "Id");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdCtgPhone,Name,Cost,Quantity,Image,Description,Configuration,Rating,UpdatedDate,UserCreatedId")] TblProduct tblProduct)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tblProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCtgPhone"] = new SelectList(_context.TblCategory, "Id", "Id", tblProduct.IdCtgPhone);
            ViewData["UserCreatedId"] = new SelectList(_context.TblUser, "Id", "Id", tblProduct.UserCreatedId);
            return View(tblProduct);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblProduct = await _context.TblProduct.FindAsync(id);
            if (tblProduct == null)
            {
                return NotFound();
            }
            ViewData["IdCtgPhone"] = new SelectList(_context.TblCategory, "Id", "Id", tblProduct.IdCtgPhone);
            ViewData["UserCreatedId"] = new SelectList(_context.TblUser, "Id", "Id", tblProduct.UserCreatedId);
            return View(tblProduct);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdCtgPhone,Name,Cost,Quantity,Image,Description,Configuration,Rating,UpdatedDate,UserCreatedId")] TblProduct tblProduct)
        {
            if (id != tblProduct.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblProductExists(tblProduct.Id))
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
            ViewData["IdCtgPhone"] = new SelectList(_context.TblCategory, "Id", "Id", tblProduct.IdCtgPhone);
            ViewData["UserCreatedId"] = new SelectList(_context.TblUser, "Id", "Id", tblProduct.UserCreatedId);
            return View(tblProduct);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblProduct = await _context.TblProduct
                .Include(t => t.IdCtgPhoneNavigation)
                .Include(t => t.UserCreated)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblProduct == null)
            {
                return NotFound();
            }

            return View(tblProduct);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tblProduct = await _context.TblProduct.FindAsync(id);
            _context.TblProduct.Remove(tblProduct);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblProductExists(int id)
        {
            return _context.TblProduct.Any(e => e.Id == id);
        }

       
    }
}
