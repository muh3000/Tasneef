using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tasneef.Core.Interfaces;
using Tasneef.Data;
using Tasneef.Models;

namespace Tasneef.Controllers
{
    [Authorize]
    public class MessagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserPermit _userPermit;
        public MessagesController(ApplicationDbContext context, IUserPermit userPermit)
        {
            _context = context;


            _userPermit = userPermit;
        }

        // GET: Messages
        public async Task<IActionResult> Index()
        {
            var custList = await _userPermit.GetPermittedCustomersAsync();

            var applicationDbContext = _context.Messages.Include(m => m.CreatedBy).Include(m => m.Project).Include(m => m.UpdatedBy)
                .Where(m=> custList.Contains( m.Project.CustomerId) );
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> Chat()
        {
            var custList = await _userPermit.GetPermittedCustomersAsync();

            var projects = await _context.Projects.Where(p=> p.ProjectStatusId == 1 && custList.Contains( p.CustomerId) ).Include(p=>p.Messages).Include(p=>p.Customer)
                .Include(p=>p.CreatedBy)
                .OrderByDescending(p => p.Messages.OrderByDescending(t => t.CreatedDate).First().CreatedDate)
                .ToListAsync<Project>();
            return View(projects);
        }


        // GET: Messages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Messages
                .Include(m => m.CreatedBy)
                .Include(m => m.Project)
                .Include(m => m.UpdatedBy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        // GET: Messages/Create
        public IActionResult Create()
        {
            ViewData["CreatedById"] = new SelectList(_context.AppUsers, "Id", "Id");
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Id");
            ViewData["UpdatedById"] = new SelectList(_context.AppUsers, "Id", "Id");
            return View();
        }

        // POST: Messages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProjectId,Body,CreatedById,CreatedDate,UpdatedById,UpdatedDate")] Message message)
        {
            if (ModelState.IsValid)
            {
                _context.Add(message);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CreatedById"] = new SelectList(_context.AppUsers, "Id", "Id", message.CreatedById);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Id", message.ProjectId);
            ViewData["UpdatedById"] = new SelectList(_context.AppUsers, "Id", "Id", message.UpdatedById);
            return View(message);
        }

        // GET: Messages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Messages.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }
            ViewData["CreatedById"] = new SelectList(_context.AppUsers, "Id", "Id", message.CreatedById);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Id", message.ProjectId);
            ViewData["UpdatedById"] = new SelectList(_context.AppUsers, "Id", "Id", message.UpdatedById);
            return View(message);
        }

        // POST: Messages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProjectId,Body,CreatedById,CreatedDate,UpdatedById,UpdatedDate")] Message message)
        {
            if (id != message.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(message);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MessageExists(message.Id))
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
            ViewData["CreatedById"] = new SelectList(_context.AppUsers, "Id", "Id", message.CreatedById);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Id", message.ProjectId);
            ViewData["UpdatedById"] = new SelectList(_context.AppUsers, "Id", "Id", message.UpdatedById);
            return View(message);
        }

        // GET: Messages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Messages
                .Include(m => m.CreatedBy)
                .Include(m => m.Project)
                .Include(m => m.UpdatedBy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var message = await _context.Messages.FindAsync(id);
            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MessageExists(int id)
        {
            return _context.Messages.Any(e => e.Id == id);
        }
    }
}
