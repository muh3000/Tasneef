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
    [Authorize(Roles = "Admin,Manager")]
    public class AuditTrailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuditTrailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AuditTrails
        public async Task<IActionResult> Index()
        {
            return View(await _context.AuditTrails.ToListAsync());
        }

        // GET: AuditTrails/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var auditTrail = await _context.AuditTrails
                .FirstOrDefaultAsync(m => m.Id == id);
            if (auditTrail == null)
            {
                return NotFound();
            }

            return View(auditTrail);
        }

        // GET: AuditTrails/Create
        
    }
}
