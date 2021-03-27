using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PhoneStore.Models;

namespace PhoneStore.Controllers
{
    public class UsersController : Controller
    {
        private readonly PhoneManagementContext _context;

        public UsersController(PhoneManagementContext context)
        {
            _context = context;
        }

        // GET: TblUsers
        public async Task<IActionResult> Index()
        {
            var phoneManagementContext = _context.TblUser.Include(t => t.IdRoleNavigation);
            return View(await phoneManagementContext.ToListAsync());
        }

        // GET: TblUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblUser = await _context.TblUser
                .Include(t => t.IdRoleNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblUser == null)
            {
                return NotFound();
            }

            return View(tblUser);
        }
        #region
        // GET: TblUsers/Create
        public IActionResult Create()
        {
            ViewData["IdRole"] = new SelectList(_context.TblRole, "Id", "Id");
            ViewData["IdNameRole"] = new SelectList(_context.TblRole, "Id", "Name");
            return View();
        }

        // POST: TblUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdRole,Fullname,Password,Phone,Address,Status")] TblUser tblUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tblUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdRole"] = new SelectList(_context.TblRole, "Id", "Id", tblUser.IdRole);
            return View(tblUser);
        }

        // GET: TblUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblUser = await _context.TblUser.FindAsync(id);
            if (tblUser == null)
            {
                return NotFound();
            }
            ViewData["IdRole"] = new SelectList(_context.TblRole, "Id", "Id", tblUser.IdRole);
            return View(tblUser);
        }

        // POST: TblUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdRole,Fullname,Password,Phone,Address,Status")] TblUser tblUser)
        {
            if (id != tblUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblUserExists(tblUser.Id))
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
            ViewData["IdRole"] = new SelectList(_context.TblRole, "Id", "Id", tblUser.IdRole);
            return View(tblUser);
        }

        // GET: TblUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblUser = await _context.TblUser
                .Include(t => t.IdRoleNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblUser == null)
            {
                return NotFound();
            }

            return View(tblUser);
        }

        // POST: TblUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tblUser = await _context.TblUser.FindAsync(id);
            _context.TblUser.Remove(tblUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblUserExists(int id)
        {
            return _context.TblUser.Any(e => e.Id == id);
        }
        #endregion


        [HttpPost]
        public IActionResult CheckLogin(TblUser user, string idProduct)
        {
            try
            {
                var users = _context.TblUser;
                if (user == null)
                {
                    ViewData["ERROR"] = "User's phone or password was wrong, try again";
                    return RedirectToAction("Index", "Home", new {id = idProduct});
                }
                foreach (TblUser u in users)
                {
                    if (user.Password.Equals(u.Password) && user.Phone.Equals(u.Phone))
                    {
                        var userInfor = JsonConvert.SerializeObject(u);
                        HttpContext.Session.SetString("USER", userInfor);
                      
                        if (!String.IsNullOrEmpty(idProduct))
                        {
                            return RedirectToAction("Details", "Products", new { id = idProduct });
                        }
                        else
                        {
                            return RedirectToAction("Index", "Products");
                        }
                        
                    }
                }
                TempData["ERROR"] = "User's phone or password was wrong, try again";                
                return RedirectToAction("Index", "Home", new { id = idProduct });
            }
            catch (Exception)
            {
                ViewData["ERROR"] = "Account did not exist !!!";
                return RedirectToAction("Index", "Home", new { id = idProduct });
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("USER");
            return RedirectToAction("Index", "Products");
        }
    }
}
