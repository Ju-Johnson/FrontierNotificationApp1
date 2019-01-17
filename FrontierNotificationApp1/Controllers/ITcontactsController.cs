using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FrontierNotificationApp1.Models;

namespace FrontierNotificationApp1.Controllers
{
    public class ITcontactsController : Controller
    {
        private readonly FrontierNotificationDBContext _context;

        public ITcontactsController(FrontierNotificationDBContext context)
        {
            _context = context;
        }

        // GET: ITcontacts
        public async Task<IActionResult> Index()
        {
            return View(await _context.ITcontacts.ToListAsync());
        }

        // GET: ITcontacts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var iTcontacts = await _context.ITcontacts
                .SingleOrDefaultAsync(m => m.Id == id);
            if (iTcontacts == null)
            {
                return NotFound();
            }

            return View(iTcontacts);
        }

        // GET: ITcontacts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ITcontacts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,name,email")] ITcontacts iTcontacts)
        {
            if (ModelState.IsValid)
            {
                _context.Add(iTcontacts);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(iTcontacts);
        }

        // GET: ITcontacts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var iTcontacts = await _context.ITcontacts.SingleOrDefaultAsync(m => m.Id == id);
            if (iTcontacts == null)
            {
                return NotFound();
            }
            return View(iTcontacts);
        }

        // POST: ITcontacts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,name,email")] ITcontacts iTcontacts)
        {
            if (id != iTcontacts.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(iTcontacts);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ITcontactsExists(iTcontacts.Id))
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
            return View(iTcontacts);
        }

        // GET: ITcontacts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var iTcontacts = await _context.ITcontacts
                .SingleOrDefaultAsync(m => m.Id == id);
            if (iTcontacts == null)
            {
                return NotFound();
            }

            return View(iTcontacts);
        }

        // POST: ITcontacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var iTcontacts = await _context.ITcontacts.SingleOrDefaultAsync(m => m.Id == id);
            _context.ITcontacts.Remove(iTcontacts);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ITcontactsExists(int id)
        {
            return _context.ITcontacts.Any(e => e.Id == id);
        }
    }
}
