using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Tasneef.Core.Interfaces;
using Tasneef.Data;

using Microsoft.EntityFrameworkCore;
using Tasneef.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Tasneef.Core.Services
{
    [Authorize]
    public class UserPermitService : IUserPermit
    {
        private readonly ApplicationDbContext _context;
        private string _userID;
        private readonly UserManager<AppUser> _userManager;

        public UserPermitService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor,UserManager<AppUser> userManager)
        {
            _context = context;
            _userID = httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _userManager = userManager;

        }

        public async Task<bool> HasPermitOnCustomerAsync(int CustomerId)
        {
            bool EmpHasGrant = await _context.EmployeeCustomerGrants.AnyAsync(e => e.CustomerId == CustomerId && e.EmployeeId == _userID);
            var user = await _userManager.Users.FirstOrDefaultAsync(u=>u.Id == _userID);
            bool AdminUser = false;
            bool CustUser = false;
            
            if(user != null) if(await _userManager.IsInRoleAsync(user, "Admin") || await _userManager.IsInRoleAsync(user, "Manager")) AdminUser = true;
            

            if(await _context.CustomerUsers.AnyAsync(c => c.CustomerId == CustomerId && c.UserId == _userID)) CustUser = true;



            return AdminUser || EmpHasGrant || CustUser;

        }

        public async Task<bool> IsInRoleAsync(string Role)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == _userID);
            bool inRole = false;
            if (user != null)
            {
                inRole = await _userManager.IsInRoleAsync(user, Role);
            }

            return inRole;
        }

        public async Task<List<int>> GetPermittedCustomersAsync()
        {
            IQueryable<Customer> customers =null;//= _context.Customers;
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == _userID);
            if (user != null) 
                if (await _userManager.IsInRoleAsync(user, "Employee"))
                {
                    var custIds = _context.EmployeeCustomerGrants.Where(e => e.EmployeeId == _userID).Select(c => c.CustomerId);
                    customers = _context.Customers.Where(c => custIds.Contains(c.Id));
                }
                else if(await _userManager.IsInRoleAsync(user, "Customer"))
                {
                    var custIds = _context.CustomerUsers.Where(e => e.UserId == _userID).Select(c => c.CustomerId);
                    customers = _context.Customers.Where(c => custIds.Contains(c.Id));
                }
                else if (await _userManager.IsInRoleAsync(user, "Admin") || await _userManager.IsInRoleAsync(user, "Manager"))
                {
                    customers = _context.Customers;
                }
            

            return customers.Select(c => c.Id).ToList();


        }

        
    }
}
