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
    public class EmailConfigurationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmailConfigurationsController(ApplicationDbContext context)
        {
            _context = context;
        }



        // GET: EmailConfigurations/Details/5
        public async Task<IActionResult> Details(string id="1")
        {
            if (id == null)
            {
                return NotFound();
            }

            var emailConfiguration = await _context.EmailConfigurations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (emailConfiguration == null)
            {
                return NotFound();
            }

            return View(emailConfiguration);
        }

        // GET: EmailConfigurations/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emailConfiguration = await _context.EmailConfigurations.FindAsync(id);
            if (emailConfiguration == null)
            {
                return NotFound();
            }
            return View(emailConfiguration);
        }

        // POST: EmailConfigurations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,FromEmail,SenderName,SMTPUername,SMTPPassword,SMTPServer,SMTPPort,EnableSSL")] EmailConfiguration emailConfiguration)
        {
            if (id != emailConfiguration.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(emailConfiguration);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmailConfigurationExists(emailConfiguration.Id))
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
            return View(emailConfiguration);
        }


        private bool EmailConfigurationExists(string id)
        {
            return _context.EmailConfigurations.Any(e => e.Id == id);
        }
    }
}
