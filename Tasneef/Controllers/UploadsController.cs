using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tasneef.Data;
using Tasneef.Models;

namespace Tasneef.Controllers
{
    [Authorize]
    public class UploadsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UploadsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Uploads
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Uploads.Include(u => u.CreatedBy).Include(u => u.Message).Include(u => u.UpdatedBy);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Uploads/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var upload = await _context.Uploads
                .Include(u => u.CreatedBy)
                .Include(u => u.Message)
                .Include(u => u.UpdatedBy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (upload == null)
            {
                return NotFound();
            }

            return View(upload);
        }

        // GET: Uploads/Create
        public IActionResult Create()
        {
            ViewData["CreatedById"] = new SelectList(_context.AppUsers, "Id", "Id");
            ViewData["MessageId"] = new SelectList(_context.Messages, "Id", "Id");
            ViewData["UpdatedById"] = new SelectList(_context.AppUsers, "Id", "Id");
            return View();
        }

        // POST: Uploads/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MessageId,FilePath,Tag,CreatedById,CreatedDate,UpdatedById,UpdatedDate")] Upload upload)
        {
            if (ModelState.IsValid)
            {
                _context.Add(upload);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CreatedById"] = new SelectList(_context.AppUsers, "Id", "Id", upload.CreatedById);
            ViewData["MessageId"] = new SelectList(_context.Messages, "Id", "Id", upload.MessageId);
            ViewData["UpdatedById"] = new SelectList(_context.AppUsers, "Id", "Id", upload.UpdatedById);
            return View(upload);
        }

        // GET: Uploads/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var upload = await _context.Uploads.FindAsync(id);
            if (upload == null)
            {
                return NotFound();
            }
            ViewData["CreatedById"] = new SelectList(_context.AppUsers, "Id", "Id", upload.CreatedById);
            ViewData["MessageId"] = new SelectList(_context.Messages, "Id", "Id", upload.MessageId);
            ViewData["UpdatedById"] = new SelectList(_context.AppUsers, "Id", "Id", upload.UpdatedById);
            return View(upload);
        }

        // POST: Uploads/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MessageId,FilePath,Tag,CreatedById,CreatedDate,UpdatedById,UpdatedDate")] Upload upload)
        {
            if (id != upload.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(upload);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UploadExists(upload.Id))
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
            ViewData["CreatedById"] = new SelectList(_context.AppUsers, "Id", "Id", upload.CreatedById);
            ViewData["MessageId"] = new SelectList(_context.Messages, "Id", "Id", upload.MessageId);
            ViewData["UpdatedById"] = new SelectList(_context.AppUsers, "Id", "Id", upload.UpdatedById);
            return View(upload);
        }

        // GET: Uploads/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var upload = await _context.Uploads
                .Include(u => u.CreatedBy)
                .Include(u => u.Message)
                .Include(u => u.UpdatedBy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (upload == null)
            {
                return NotFound();
            }

            return View(upload);
        }

        // POST: Uploads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var upload = await _context.Uploads.FindAsync(id);
            _context.Uploads.Remove(upload);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UploadExists(int id)
        {
            return _context.Uploads.Any(e => e.Id == id);
        }
    }
}
