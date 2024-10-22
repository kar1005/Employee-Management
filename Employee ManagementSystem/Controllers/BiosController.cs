using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Employee_ManagementSystem.Data;
using Employee_ManagementSystem.Models;

namespace Employee_ManagementSystem.Controllers
{
    public class BiosController : Controller
    {
        private readonly ApplicationDbcontext _context;
        private readonly ILogger<BiosController> _logger;


        public BiosController(ApplicationDbcontext context, ILogger<BiosController> logger)
        {
            _context = context;
            _logger = logger;

        }

        // GET: Bios
        public async Task<IActionResult> Index()
        {
            var applicationDbcontext = _context.BioData.Include(b => b.Employee);
            return View(await applicationDbcontext.ToListAsync());
        }

        // GET: Bios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bio = await _context.BioData
                .Include(b => b.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bio == null)
            {
                return NotFound();
            }

            return View(bio);
        }

        // GET: Bios/Create
        public IActionResult Create()
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName");
            return View();
        }

        // POST: Bios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeId,DateOfBirth,Aadhar,EmergencyContact,Qualifications,Hobbies,SocialHandles")] Bio bio)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Using EF Core to check if employee exists and bio doesn't exist in a single query
                    var employeeCheck = await _context.Employees
                        .Select(e => new
                        {
                            e.Id,
                            HasBio = _context.BioData.Any(b => b.EmployeeId == e.Id)
                        })
                        .FirstOrDefaultAsync(e => e.Id == bio.EmployeeId);

                    if (employeeCheck == null)
                    {
                        ModelState.AddModelError("EmployeeId", "Selected employee does not exist.");
                        await PrepareEmployeeSelectList();
                        return View(bio);
                    }

                    if (employeeCheck.HasBio)
                    {
                        ModelState.AddModelError("EmployeeId", "Bio data already exists for this employee.");
                        await PrepareEmployeeSelectList();
                        return View(bio);
                    }

                    // Use explicit transaction for multiple operations
                    using var transaction = await _context.Database.BeginTransactionAsync();
                    try
                    {
                        _context.BioData.Add(bio);
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();

                        TempData["Success"] = "Bio data created successfully!";
                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }

                _logger.LogWarning("Model state is invalid: {Errors}",
                    string.Join("; ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)));
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error while creating bio data");
                ModelState.AddModelError("", "A database error occurred. Please try again.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating bio data");
                ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
            }

            await PrepareEmployeeSelectList();
            return View(bio);
        }

        private async Task PrepareEmployeeSelectList()
        {
            var employeesWithoutBio = await _context.Employees
                .Where(e => !_context.BioData.Any(b => b.EmployeeId == e.Id))
                .Select(e => new { Id = e.Id, FullName = e.FullName })
                .ToListAsync();

            ViewData["EmployeeId"] = new SelectList(employeesWithoutBio, "Id", "FullName");
        }

        // GET: Bios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bio = await _context.BioData.FindAsync(id);
            if (bio == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", bio.EmployeeId);
            return View(bio);
        }

        // POST: Bios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EmployeeId,DateOfBirth,Aadhar,EmergencyContact,Qualifications,Hobbies,SocialHandles")] Bio bio)
        {
            if (id != bio.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BioExists(bio.Id))
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
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", bio.EmployeeId);
            return View(bio);
        }

        // GET: Bios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bio = await _context.BioData
                .Include(b => b.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bio == null)
            {
                return NotFound();
            }

            return View(bio);
        }

        // POST: Bios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bio = await _context.BioData.FindAsync(id);
            if (bio != null)
            {
                _context.BioData.Remove(bio);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BioExists(int id)
        {
            return _context.BioData.Any(e => e.Id == id);
        }
    }
}
