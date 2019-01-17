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
    public class ITteamsController : Controller
    {
        private readonly FrontierNotificationDBContext _context;

        public ITteamsController(FrontierNotificationDBContext context)
        {
            _context = context;
        }

        // GET: ITteams
        public async Task<IActionResult> Index()
        {
            return View(await _context.ITteams.ToListAsync());
        }

        // GET: ITteams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var iTteams = await _context.ITteams
                .SingleOrDefaultAsync(m => m.Id == id);
            if (iTteams == null)
            {
                return NotFound();
            }

            return View(iTteams);
        }

        // GET: ITteams/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ITteams/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,name")] ITteams iTteams)
        {
            if (ModelState.IsValid)
            {
                _context.Add(iTteams);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(iTteams);
        }

        // GET: ITteams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var iTteams = await _context.ITteams.SingleOrDefaultAsync(m => m.Id == id);
            if (iTteams == null)
            {
                return NotFound();
            }
            return View(iTteams);
        }

        // POST: ITteams/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,name")] ITteams iTteams)
        {
            if (id != iTteams.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(iTteams);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ITteamsExists(iTteams.Id))
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
            return View(iTteams);
        }

        // GET: ITteams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var iTteams = await _context.ITteams
                .SingleOrDefaultAsync(m => m.Id == id);
            if (iTteams == null)
            {
                return NotFound();
            }

            return View(iTteams);
        }

        // POST: ITteams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var iTteams = await _context.ITteams.SingleOrDefaultAsync(m => m.Id == id);
            _context.ITteams.Remove(iTteams);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ITteamsExists(int id)
        {
            return _context.ITteams.Any(e => e.Id == id);
        }
    }
}
