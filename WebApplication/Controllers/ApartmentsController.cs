using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Application;
using Application.Entities;
using Microsoft.AspNetCore.Mvc.Filters;
using Application.IServices;

namespace WebApplication.Controllers
{
    public class ApartmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IUserService _us;
        private User user;

        public ApartmentsController(ApplicationDbContext context, IUserService us)
        {
            _context = context;
            _us = us;
        }
        public override void OnActionExecuting(ActionExecutingContext ctx)
        {
            base.OnActionExecuting(ctx);
            user = _us.GetUserFromRequest(Request);
            if (user == null)
                ViewBag.Name = "";
            else ViewBag.Name = user.Name + " " + user.Surname;
        }

        // GET: Apartments
        public async Task<IActionResult> Index()
        {
            return View(await _context.Apartment.Include(p => p.Office).ToListAsync());
        }

        // GET: Apartments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apartment = await _context.Apartment
                .FirstOrDefaultAsync(m => m.Id == id);
            if (apartment == null)
            {
                return NotFound();
            }
            
            return RedirectToAction("Index", "ApartmentRooms", new {apartment = apartment.Id});
        }

        // GET: Apartments/Create
        public IActionResult Create()
        {
            var offices = OfficeList();
            var values = from ofc in offices
                         select ofc.Text;
            ViewBag.Offices = values;
            return View();
        }

        // POST: Apartments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string officeTitle, Apartment apartment)
        {
            
                var office = _context.Office.SingleOrDefault(x => x.Name == officeTitle);
                apartment.OfficeId = office.Id;
                _context.Add(apartment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            
        }

        // GET: Apartments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apartment = await _context.Apartment.FindAsync(id);
            var office = await _context.Office.FindAsync(apartment.OfficeId);
            apartment.Office = office;
            if (apartment == null)
            {
                return NotFound();
            }
            var offices = OfficeList();         
            var values = from ofc in offices
                         select ofc.Text;
            ViewBag.Offices = values;
            return View(apartment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string officeTitle, Apartment apartment)
        {         
                try
                {
                    var office =  _context.Office.SingleOrDefault(x => x.Name == officeTitle);
                    apartment.OfficeId = office.Id;
                    _context.Update(apartment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApartmentExists(apartment.Id))
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

        // GET: Apartments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apartment = await _context.Apartment
                .FirstOrDefaultAsync(m => m.Id == id);
            var office = _context.Office.SingleOrDefault(x => x.Id == apartment.OfficeId);
            apartment.Office = office;
            if (apartment == null)
            {
                return NotFound();
            }

            return View(apartment);
        }

        // POST: Apartments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var apartment = await _context.Apartment.FindAsync(id);
            _context.Apartment.Remove(apartment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApartmentExists(int id)
        {
            return _context.Apartment.Any(e => e.Id == id);
        }

        public List<SelectListItem> OfficeList()
        {
            List<SelectListItem> listItems = new List<SelectListItem>();
            var offices = _context.Office.ToList();
            foreach (var officeName in offices)
            {
                listItems.Add(new SelectListItem { Text = officeName.Name, Value = officeName.Name });
            }
            return listItems;
        }
    }
}
