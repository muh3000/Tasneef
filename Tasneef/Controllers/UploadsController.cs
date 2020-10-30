using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
        private readonly string _userID;
        private IHostingEnvironment _hostingEnvironment;

        public UploadsController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _userID = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Uploads
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Uploads.Include(u => u.CreatedBy).Include(u => u.Customer).Include(u => u.Message).Include(u => u.Project).Include(u => u.UpdatedBy);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> UploadForm(int id)
        {
            var uploadModel = _context.Uploads.FindAsync(id);//.Include(u => u.CreatedBy).Include(u => u.Customer).Include(u => u.Message).Include(u => u.Project).Include(u => u.UpdatedBy);
            return View(await uploadModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadFile(int id,IFormFile file)
        {
            var fileSaveName = await UploadFileAsync(file, "Uploads");
            Upload upload = await _context.Uploads.FindAsync(id);
            upload.UpdatedById = _userID;
            upload.UpdatedDate = DateTime.Now;
            upload.FilePath = fileSaveName;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));



            return View(nameof(UploadForm),new { id = id });
        }

        public async Task<string> UploadFileAsync(IFormFile file,string DestFolder)
        {
            string fileSaveName = "";
            //string folderName = DestFolder;
            string webRootPath = _hostingEnvironment.WebRootPath;
            string uploadFolder = Path.Combine(webRootPath, DestFolder);

            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }
            if (file.Length > 0)
            {
                string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                Random rand = new Random();
                string fullPath = "";

                while (true)
                {
                    fileSaveName = rand.Next(1000000,10000000).ToString() + sFileExtension;
                    fullPath = Path.Combine(uploadFolder, fileSaveName);
                    if (!System.IO.File.Exists(fullPath))
                    {
                        break;
                    }
                }
                //fullPath = Path.GetTempFileName();
                //var stream = System.IO.File.Create(fullPath)
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    
                    await file.CopyToAsync(stream);




                }
            }
            return fileSaveName;

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
                .Include(u => u.Customer)
                .Include(u => u.Message)
                .Include(u => u.Project)
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
            ViewData["CreatedById"] = new SelectList(_context.AppUsers, "Id", "Name");
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name");
            ViewData["MessageId"] = new SelectList(_context.Messages, "Id", "Id");
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name");
            ViewData["UpdatedById"] = new SelectList(_context.AppUsers, "Id", "Name");
            return View();
        }

        // POST: Uploads/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Upload upload,IFormFile file)
        {
            //var savedFileName = "";
            var savedFileName = await UploadFileAsync(file, "Uploads") ;
            if (ModelState.IsValid)
            {
                upload.CreatedById = _userID;
                upload.CreatedDate = DateTime.Now;
                upload.FilePath = savedFileName;
                _context.Add(upload);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CreatedById"] = new SelectList(_context.AppUsers, "Id", "Name", upload.CreatedById);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name", upload.CustomerId);
            ViewData["MessageId"] = new SelectList(_context.Messages, "Id", "Id", upload.MessageId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", upload.ProjectId);
            ViewData["UpdatedById"] = new SelectList(_context.AppUsers, "Id", "Name", upload.UpdatedById);
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
            ViewData["CreatedById"] = new SelectList(_context.AppUsers, "Id", "Name", upload.CreatedById);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name", upload.CustomerId);
            ViewData["MessageId"] = new SelectList(_context.Messages, "Id", "Id", upload.MessageId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", upload.ProjectId);
            ViewData["UpdatedById"] = new SelectList(_context.AppUsers, "Id", "Name", upload.UpdatedById);
            return View(upload);
        }

        // POST: Uploads/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MessageId,ProjectId,CustomerId,FilePath,Tag,CreatedById,CreatedDate,UpdatedById,UpdatedDate")] Upload upload)
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
            ViewData["CreatedById"] = new SelectList(_context.AppUsers, "Id", "Name", upload.CreatedById);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name", upload.CustomerId);
            ViewData["MessageId"] = new SelectList(_context.Messages, "Id", "Id", upload.MessageId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", upload.ProjectId);
            ViewData["UpdatedById"] = new SelectList(_context.AppUsers, "Id", "Name", upload.UpdatedById);
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
                .Include(u => u.Customer)
                .Include(u => u.Message)
                .Include(u => u.Project)
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
