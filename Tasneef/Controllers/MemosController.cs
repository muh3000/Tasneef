using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tasneef.Core.Interfaces;
using Tasneef.Data;
using Tasneef.Models;

namespace Tasneef.Controllers
{
    [Authorize]
    public class MemosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;
        private readonly INotificationService _notificationService;
        private string _userID;
        public MemosController(ApplicationDbContext context, IEmailSender emailSender, INotificationService notificationService,IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _emailSender = emailSender;
            _notificationService = notificationService;
            _userID = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        // GET: Memos
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Memos.Include(m => m.CreatedBy).Include(m => m.UpdatedBy);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Memos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var memo = await _context.Memos
                .Include(m => m.CreatedBy)
                .Include(m => m.UpdatedBy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (memo == null)
            {
                return NotFound();
            }
            var notifications = await _notificationService.GetNotificationsListAsync();
            var notify =  notifications.FirstOrDefault(n => n.Entity == "Memos" && n.EntityId == memo.Id.ToString() && n.UserId == _userID);
            if (notify!=null)
                await _notificationService.MarkNotificationAsReadAsync(notify);
            var customerUser = await _context.CustomerUsers.FirstOrDefaultAsync(u => u.UserId == _userID);
            if (customerUser != null) {
                var customerMemo = await _context.CustomerMemos.FirstOrDefaultAsync(c => c.MemoId == memo.Id && c.CustomerId == customerUser.CustomerId && c.IsRead == false );
                if (customerMemo != null)
                {
                    customerMemo.IsRead = true;
                    customerMemo.ReadDate = DateTime.Now;
                    await _context.SaveChangesAsync();
                }

            }
            return View(memo);
        }

        // GET: Memos/Create
        public IActionResult Create()
        {
            ViewData["CreatedById"] = new SelectList(_context.AppUsers, "Id", "Id");
            ViewData["UpdatedById"] = new SelectList(_context.AppUsers, "Id", "Id");
            return View();
        }

        // POST: Memos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Body,CreatedById,CreatedDate,UpdatedById,UpdatedDate")] Memo memo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(memo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CreatedById"] = new SelectList(_context.AppUsers, "Id", "Id", memo.CreatedById);
            ViewData["UpdatedById"] = new SelectList(_context.AppUsers, "Id", "Id", memo.UpdatedById);
            return View(memo);
        }

        // GET: Memos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var memo = await _context.Memos.FindAsync(id);
            if (memo == null)
            {
                return NotFound();
            }
            ViewData["CreatedById"] = new SelectList(_context.AppUsers, "Id", "Id", memo.CreatedById);
            ViewData["UpdatedById"] = new SelectList(_context.AppUsers, "Id", "Id", memo.UpdatedById);
            return View(memo);
        }

        // POST: Memos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Body,CreatedById,CreatedDate,UpdatedById,UpdatedDate")] Memo memo)
        {
            if (id != memo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(memo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemoExists(memo.Id))
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
            ViewData["CreatedById"] = new SelectList(_context.AppUsers, "Id", "Id", memo.CreatedById);
            ViewData["UpdatedById"] = new SelectList(_context.AppUsers, "Id", "Id", memo.UpdatedById);
            return View(memo);
        }

        // GET: Memos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var memo = await _context.Memos
                .Include(m => m.CreatedBy)
                .Include(m => m.UpdatedBy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (memo == null)
            {
                return NotFound();
            }

            return View(memo);
        }

        // POST: Memos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var memo = await _context.Memos.FindAsync(id);
            _context.Memos.Remove(memo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MemoExists(int id)
        {
            return _context.Memos.Any(e => e.Id == id);
        }



        public IActionResult CreateCustomerMemos()
        {
            ViewData["CreatedById"] = new SelectList(_context.AppUsers, "Id", "Id");
            ViewData["UpdatedById"] = new SelectList(_context.AppUsers, "Id", "Id");
            ViewData["Customers"] = new MultiSelectList(_context.Customers.Where(c=>c.Subscriptions.Any(s=>s.EndDate >= DateTime.Now)), "Id", "Name");
            return View();
        }

        // POST: Memos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCustomerMemos(Memo memo,int[] customersList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(memo);
                await _context.SaveChangesAsync();
                foreach(var customer in customersList)
                {
                    CustomerMemo cusomerMemo = new CustomerMemo()
                    {
                        CustomerId = customer,
                        MemoId = memo.Id,
                        CreatedDate = DateTime.Now
                    };
                    _context.Add(cusomerMemo);
                    await _context.SaveChangesAsync();
                    Customer cust = await _context.Customers.Include(c=>c.CustomerUsers).FirstAsync(c=>c.Id == customer);
                    await _notificationService.CreateCustomerNotificationsAsync("Memos", memo.Id.ToString(), cust, memo.Title);
                    await SendNotificationEmailAsync(cust, "New Memeo from Tasneef", "Please login to system to check the new Memo");

                }



                return RedirectToAction(nameof(Index));
            }
            ViewData["CreatedById"] = new SelectList(_context.AppUsers, "Id", "Id", memo.CreatedById);
            ViewData["UpdatedById"] = new SelectList(_context.AppUsers, "Id", "Id", memo.UpdatedById);
            return View(memo);
        }

        public async Task<int> SendNotificationEmailAsync(Customer customer, string Subject, string Body)
        {
            foreach (var customerUser in customer.CustomerUsers)
            {
                await _emailSender.SendEmailAsync(customerUser.User.Email, Subject, Body);
            }



            return 0;
        }
        


    }
}
