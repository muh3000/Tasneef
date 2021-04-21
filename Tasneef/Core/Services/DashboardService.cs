using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Tasneef.Core.Interfaces;
using Tasneef.Data;

namespace Tasneef.Core.Services
{
    public class DashboardService : IDashboard
    {
        private readonly ApplicationDbContext _context;
        private string _userID;
        private readonly IUserPermit _userPermit;
        public DashboardService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IUserPermit userPermit)
        {
            _userID = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            _context = context;
            _userPermit = userPermit;
        }
        public async Task<int> GetNumberOfActiveProjects()
        {
            var count = _context.Projects.Where(p => p.ProjectStatusId == 1).Count();
            return count;

        }

        public async Task<int> GetNumberOfNotifications()
        {
            return _context.Notifications.Where(n => n.Read == false && n.UserId == _userID).Count();
        }

        public async Task<int> GetNumberOfPendingUploads()
        {
            var custList = await _userPermit.GetPermittedCustomersAsync();
            return _context.Uploads.Where(u => u.FilePath == "" && custList.Contains( u.Project.CustomerId)).Count();
        }

        public async Task<int> GetNumberofActiveSubscriptions()
        {
            var custList = await _userPermit.GetPermittedCustomersAsync();
            return  _context.Subscriptions.Where(s => custList.Contains(s.CustomerId) && s.EndDate > DateTime.Now).Count();
        }
    }
}
