using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tasneef.Data;
using Tasneef.Models;

namespace Tasneef.Controllers
{
    public class EmployeeCustomerGrantsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeCustomerGrantsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: EmployeeCustomerGrants
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.EmployeeCustomerGrants.Include(e => e.Customer).Include(e => e.Employee);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: EmployeeCustomerGrants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employeeCustomerGrant = await _context.EmployeeCustomerGrants
                .Include(e => e.Customer)
                .Include(e => e.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employeeCustomerGrant == null)
            {
                return NotFound();
            }

            return View(employeeCustomerGrant);
        }

        // GET: EmployeeCustomerGrants/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name");
            ViewData["EmployeeId"] = new SelectList(_context.AppUsers, "Id", "Name");
            return View();
        }

        // POST: EmployeeCustomerGrants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EmployeeId,CustomerId")] EmployeeCustomerGrant employeeCustomerGrant)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employeeCustomerGrant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", employeeCustomerGrant.CustomerId);
            ViewData["EmployeeId"] = new SelectList(_context.AppUsers, "Id", "Id", employeeCustomerGrant.EmployeeId);
            return View(employeeCustomerGrant);
        }

        // GET: EmployeeCustomerGrants/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employeeCustomerGrant = await _context.EmployeeCustomerGrants.FindAsync(id);
            if (employeeCustomerGrant == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", employeeCustomerGrant.CustomerId);
            ViewData["EmployeeId"] = new SelectList(_context.AppUsers, "Id", "Id", employeeCustomerGrant.EmployeeId);
            return View(employeeCustomerGrant);
        }

        // POST: EmployeeCustomerGrants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EmployeeId,CustomerId")] EmployeeCustomerGrant employeeCustomerGrant)
        {
            if (id != employeeCustomerGrant.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employeeCustomerGrant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeCustomerGrantExists(employeeCustomerGrant.Id))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", employeeCustomerGrant.CustomerId);
            ViewData["EmployeeId"] = new SelectList(_context.AppUsers, "Id", "Id", employeeCustomerGrant.EmployeeId);
            return View(employeeCustomerGrant);
        }

        // GET: EmployeeCustomerGrants/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employeeCustomerGrant = await _context.EmployeeCustomerGrants
                .Include(e => e.Customer)
                .Include(e => e.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employeeCustomerGrant == null)
            {
                return NotFound();
            }

            return View(employeeCustomerGrant);
        }

        // POST: EmployeeCustomerGrants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employeeCustomerGrant = await _context.EmployeeCustomerGrants.FindAsync(id);
            _context.EmployeeCustomerGrants.Remove(employeeCustomerGrant);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeCustomerGrantExists(int id)
        {
            return _context.EmployeeCustomerGrants.Any(e => e.Id == id);
        }
    }
}
