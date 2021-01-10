using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tasneef.Data;
using Tasneef.Models;
using Tasneef.ViewModels;

namespace Tasneef.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public UsersController(ApplicationDbContext context,UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var usersList = _userManager.Users;
            List<AppUserVM> usersVMList = new List<AppUserVM>();
            foreach(var user in usersList)
            {
                usersVMList.Add(new AppUserVM()
                {
                    Id= user.Id,
                    Name = user.Name,
                    Username = user.UserName,
//                    CustomerId = user.CustomerId,
                    //CustomerName = (await _context.Customers.FindAsync(user.CustomerId))?.Name,

                });
                
            }
            foreach(var item in usersVMList)
            {
                CustomerUser custUser = await _context.CustomerUsers.Include(c=>c.Customer).FirstOrDefaultAsync(c => c.UserId == item.Id);
                if (custUser != null)
                {
                    item.CustomerId = custUser.CustomerId;
                    item.CustomerName = custUser.Customer.Name;
                }
            }


            return View(usersVMList);
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AppUser user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }
            Customer cust = await GetCustomerByUser(id);
            var appUserVM = new AppUserVM()
            {
                Id = user.Id,
                Name = user.Name,
                Username = user.UserName,
                CustomerId = cust?.Id,
                CustomerName = cust?.Name,
                Mobile = user.PhoneNumber

            };




            return View(appUserVM);
        }

        private async Task<Customer> GetCustomerByUser(string id)
        {
            return (await _context.CustomerUsers.Include(c => c.Customer).FirstOrDefaultAsync(c => c.UserId == id))?.Customer;
        }


        // GET: Users/Create
        public IActionResult Create()
        {
            ViewData["Customers"] = new SelectList(_context.Customers, "Id", "Name");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,CustomerId,CustomerName,Username,Password,Mobile,RoleId,RoleName")] AppUserVM appUserVM)
        {
            if (ModelState.IsValid)
            {
                AppUser newUser = new AppUser()
                {
                    Name = appUserVM.Name,
                    UserName = appUserVM.Username,
                    Email = appUserVM.Username,
                    NormalizedEmail = appUserVM.Username.ToUpper(),
                    PhoneNumber = appUserVM.Mobile

                    
                };

                await _userManager.CreateAsync(newUser, appUserVM.Password);
                appUserVM.Id = newUser.Id;
                await UpdateCustomerUser(appUserVM);
                return RedirectToAction(nameof(Details),new { id = newUser.Id});
            }
            return View(appUserVM);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AppUser user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }
            Customer cust = await GetCustomerByUser(id);
            var appUserVM = new AppUserVM()
            {
                Id = user.Id,
                Name = user.Name,
                Username = user.UserName,
                CustomerId = cust?.Id,
                Mobile = user.PhoneNumber


            };
            if (appUserVM == null)
            {
                return NotFound();
            }
            ViewData["Customers"] = new SelectList(_context.Customers, "Id", "Name",cust?.Id);

            return View(appUserVM);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,CustomerId,CustomerName,Username,Password,Mobile,RoleId,RoleName")] AppUserVM appUserVM)
        {
            if (id != appUserVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    AppUser user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
                    user.Name = appUserVM.Name;

                    user.PhoneNumber = appUserVM.Mobile;
                    if (appUserVM.Password != null)
                    {
                        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                        await _userManager.ResetPasswordAsync(user, token, appUserVM.Password);
                    }
                    await UpdateCustomerUser( appUserVM);
                    await _userManager.UpdateAsync(user);

                    //_context.Update(appUserVM);
                    //await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await AppUserVMExistsAsync(appUserVM.Id))
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
            return View(appUserVM);
        }

        private async Task UpdateCustomerUser( AppUserVM appUserVM)
        {
            var customerUser = await _context.CustomerUsers.FirstOrDefaultAsync(c => c.UserId == appUserVM.Id);
            if (appUserVM.CustomerId != null)
            {
                
                if (customerUser != null)
                {
                    customerUser.CustomerId = (int)appUserVM.CustomerId;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    CustomerUser newCustUser = new CustomerUser();
                    newCustUser.CustomerId = appUserVM.CustomerId.Value;
                    newCustUser.UserId = appUserVM.Id;
                    await _context.CustomerUsers.AddAsync(newCustUser);
                    
                }

            }
            else
            {
                if(customerUser!= null)
                {
                    _context.CustomerUsers.Remove(customerUser);
                }
            }
            await _context.SaveChangesAsync();
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AppUser user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }
            Customer cust = await GetCustomerByUser(id);
            var appUserVM = new AppUserVM()
            {
                Id = user.Id,
                Name = user.Name,
                Username = user.UserName,
                CustomerId = cust?.Id,
                CustomerName = cust?.Name

            };
            if (appUserVM == null)
            {
                return NotFound();
            }

            return View(appUserVM);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            AppUser user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            await _userManager.DeleteAsync(user);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> AppUserVMExistsAsync(string id)
        {
            AppUser user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                return false;
            return true;
            
        }
    }
}
