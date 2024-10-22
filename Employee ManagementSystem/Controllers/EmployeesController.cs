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
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbcontext _context;

        public EmployeesController(ApplicationDbcontext context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            var applicationDbcontext = _context.Employees.Include(e => e.Country).Include(e => e.Deparment).Include(e => e.Job).Include(e => e.Rating).Include(e => e.Supervisor);
            return View(await applicationDbcontext.ToListAsync());
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Country)
                .Include(e => e.Deparment)
                .Include(e => e.Job)
                .Include(e => e.Rating)
                .Include(e => e.Supervisor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "CountryName");
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "DeparmentName");
            ViewData["JobId"] = new SelectList(_context.Jobs, "Id", "JobTitle");
            ViewData["RatingId"] = new SelectList(_context.Ratings, "Id", "RatingType");
            ViewData["SupervisorId"] = new SelectList(_context.Supervisors, "Id", "Name");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "CountryName", employee.CountryId);
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "DeparmentName", employee.DepartmentId);
            ViewData["JobId"] = new SelectList(_context.Jobs, "Id", "JobTitle", employee.JobId);
            ViewData["RatingId"] = new SelectList(_context.Ratings, "Id", "RatingType", employee.RatingId);
            ViewData["SupervisorId"] = new SelectList(_context.Supervisors, "Id", "Name", employee.SupervisorId);
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "CountryName", employee.CountryId);
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "DeparmentName", employee.DepartmentId);
            ViewData["JobId"] = new SelectList(_context.Jobs, "Id", "JobTitle", employee.JobId);
            ViewData["RatingId"] = new SelectList(_context.Ratings, "Id", "RatingType", employee.RatingId);
            ViewData["SupervisorId"] = new SelectList(_context.Supervisors, "Id", "Name", employee.SupervisorId);
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
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
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "CountryName", employee.CountryId);
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "DeparmentName", employee.DepartmentId);
            ViewData["JobId"] = new SelectList(_context.Jobs, "Id", "JobTitle", employee.JobId);
            ViewData["RatingId"] = new SelectList(_context.Ratings, "Id", "RatingType", employee.RatingId);
            ViewData["SupervisorId"] = new SelectList(_context.Supervisors, "Id", "Name", employee.SupervisorId);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Country)
                .Include(e => e.Deparment)
                .Include(e => e.Job)
                .Include(e => e.Rating)
                .Include(e => e.Supervisor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
