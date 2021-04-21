using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tasneef.Core.Interfaces;
using Tasneef.Data;
using Tasneef.Models;

namespace Tasneef.Controllers
{
    [Authorize(Roles = "Admin,Manager,Employee")]
    public class CustomerMemosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private string _userID;
        private readonly IUserPermit _userPermit;
        private readonly INotificationService _notificationService;
        public CustomerMemosController(ApplicationDbContext context, IUserPermit userPermit, IHttpContextAccessor httpContextAccessor,INotificationService notificationService)
        {
            _context = context;
            _userID = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            _userPermit = userPermit;
            _notificationService = notificationService;
        }

        // GET: CustomerMemoes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CustomerMemos
                .Include(c => c.CreatedBy)
                .Include(c => c.Customer)
                .Include(c => c.Memo)
                .Include(c => c.Subscription)
                .Include(c => c.UpdatedBy);
                //.Where(async c => await _userPermit.HasPermitOnCustomerAsync(c.CustomerId));

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: CustomerMemoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerMemo = await _context.CustomerMemos
                .Include(c => c.CreatedBy)
                .Include(c => c.Customer)
                .Include(c => c.Memo)
                .Include(c => c.Subscription)
                .Include(c => c.UpdatedBy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customerMemo == null)
            {
                return NotFound();
            }
            if (!await _userPermit.HasPermitOnCustomerAsync(customerMemo.CustomerId)) return Unauthorized();

            return View(customerMemo);
        }

        // GET: CustomerMemoes/Create
        public IActionResult Create()
        {
            ViewData["CreatedById"] = new SelectList(_context.AppUsers, "Id", "Id");
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id");
            ViewData["MemoId"] = new SelectList(_context.Memos, "Id", "Id");
            ViewData["SubscriptionId"] = new SelectList(_context.Subscriptions, "Id", "Id");
            ViewData["UpdatedById"] = new SelectList(_context.AppUsers, "Id", "Id");
            return View();
        }

        // POST: CustomerMemoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CustomerId,IsRead,ReadDate,SubscriptionId,MemoId,CreatedById,CreatedDate,UpdatedById,UpdatedDate")] CustomerMemo customerMemo)
        {

            if (ModelState.IsValid)
            {
                if (await _userPermit.HasPermitOnCustomerAsync(customerMemo.CustomerId))
                {
                    _context.Add(customerMemo);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                    ViewData["Error"] = "User has No access grant on this customer";
            }
            
            ViewData["CreatedById"] = new SelectList(_context.AppUsers, "Id", "Id", customerMemo.CreatedById);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", customerMemo.CustomerId);
            ViewData["MemoId"] = new SelectList(_context.Memos, "Id", "Id", customerMemo.MemoId);
            ViewData["SubscriptionId"] = new SelectList(_context.Subscriptions, "Id", "Id", customerMemo.SubscriptionId);
            ViewData["UpdatedById"] = new SelectList(_context.AppUsers, "Id", "Id", customerMemo.UpdatedById);
            return View(customerMemo);
        }

        // GET: CustomerMemoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerMemo = await _context.CustomerMemos.FindAsync(id);
            if (customerMemo == null)
            {
                return NotFound();
            }
            
            if (! await _userPermit.HasPermitOnCustomerAsync(customerMemo.CustomerId)) return NotFound();

            ViewData["CreatedById"] = new SelectList(_context.AppUsers, "Id", "Id", customerMemo.CreatedById);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", customerMemo.CustomerId);
            ViewData["MemoId"] = new SelectList(_context.Memos, "Id", "Id", customerMemo.MemoId);
            ViewData["SubscriptionId"] = new SelectList(_context.Subscriptions, "Id", "Id", customerMemo.SubscriptionId);
            ViewData["UpdatedById"] = new SelectList(_context.AppUsers, "Id", "Id", customerMemo.UpdatedById);
            return View(customerMemo);
        }

        // POST: CustomerMemoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CustomerId,IsRead,ReadDate,SubscriptionId,MemoId,CreatedById,CreatedDate,UpdatedById,UpdatedDate")] CustomerMemo customerMemo)
        {
            if (id != customerMemo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (await _userPermit.HasPermitOnCustomerAsync(customerMemo.CustomerId))
                {
                    try
                    {
                        _context.Update(customerMemo);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!CustomerMemoExists(customerMemo.Id))
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
                else
                    ViewData["Error"] = "User has No access grant on this customer";
            }
            ViewData["CreatedById"] = new SelectList(_context.AppUsers, "Id", "Id", customerMemo.CreatedById);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", customerMemo.CustomerId);
            ViewData["MemoId"] = new SelectList(_context.Memos, "Id", "Id", customerMemo.MemoId);
            ViewData["SubscriptionId"] = new SelectList(_context.Subscriptions, "Id", "Id", customerMemo.SubscriptionId);
            ViewData["UpdatedById"] = new SelectList(_context.AppUsers, "Id", "Id", customerMemo.UpdatedById);
            return View(customerMemo);
        }

        // GET: CustomerMemoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerMemo = await _context.CustomerMemos
                .Include(c => c.CreatedBy)
                .Include(c => c.Customer)
                .Include(c => c.Memo)
                .Include(c => c.Subscription)
                .Include(c => c.UpdatedBy)
                .FirstOrDefaultAsync(m => m.Id == id);


            if (customerMemo == null)
            {
                return NotFound();
            }
            if (! await _userPermit.HasPermitOnCustomerAsync(customerMemo.CustomerId)) {
                return NotFound();
            }

            return View(customerMemo);
        }

        // POST: CustomerMemoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (await _userPermit.HasPermitOnCustomerAsync(id))
            {
                var customerMemo = await _context.CustomerMemos.Include(c=>c.Customer).FirstAsync(c=>c.Id == id);

                await _notificationService.DeleteCustomerNotificationsAsync("Memos", customerMemo.MemoId.ToString(), customerMemo.Customer);

                _context.CustomerMemos.Remove(customerMemo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
                ViewData["Error"] = "User has No access grant on this customer";
            return View(nameof(Delete), new { id = id });
        }

        private bool CustomerMemoExists(int id)
        {
            return _context.CustomerMemos.Any(e => e.Id == id);
        }
    }
}
