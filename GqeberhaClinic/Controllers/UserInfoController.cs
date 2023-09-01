using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GqeberhaClinic.Data;
using GqeberhaClinic.Models;
using Microsoft.AspNetCore.Authorization;

namespace GqeberhaClinic.Controllers
{
    [Authorize]
    public class UserInfoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserInfoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UserInfo
        public async Task<IActionResult> Index()
        {
              return _context.users != null ? 
                          View(await _context.users.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.users'  is null.");
        }

        // GET: UserInfo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.users == null)
            {
                return NotFound();
            }

            var userInformation = await _context.users
                .FirstOrDefaultAsync(m => m.userId == id);
            if (userInformation == null)
            {
                return NotFound();
            }

            return View(userInformation);
        }

        // GET: UserInfo/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserInfo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("userId,FirstName,LastName,PhoneNumber,Email,Address")] UserInformation userInformation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userInformation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userInformation);
        }

        // GET: UserInfo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.users == null)
            {
                return NotFound();
            }

            var userInformation = await _context.users.FindAsync(id);
            if (userInformation == null)
            {
                return NotFound();
            }
            return View(userInformation);
        }

        // POST: UserInfo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("userId,FirstName,LastName,PhoneNumber,Email,Address")] UserInformation userInformation)
        {
            if (id != userInformation.userId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userInformation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserInformationExists(userInformation.userId))
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
            return View(userInformation);
        }

        // GET: UserInfo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.users == null)
            {
                return NotFound();
            }

            var userInformation = await _context.users
                .FirstOrDefaultAsync(m => m.userId == id);
            if (userInformation == null)
            {
                return NotFound();
            }

            return View(userInformation);
        }

        // POST: UserInfo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.users == null)
            {
                return Problem("Entity set 'ApplicationDbContext.users'  is null.");
            }
            var userInformation = await _context.users.FindAsync(id);
            if (userInformation != null)
            {
                _context.users.Remove(userInformation);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserInformationExists(int id)
        {
          return (_context.users?.Any(e => e.userId == id)).GetValueOrDefault();
        }
    }
}
