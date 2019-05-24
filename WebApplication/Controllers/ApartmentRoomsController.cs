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
    public class ApartmentRoomsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IUserService _us;
        private User user;

        public ApartmentRoomsController(ApplicationDbContext context, IUserService us)
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

        // GET: ApartmentRooms
        public async Task<IActionResult> Index(int apartment)
        {
            //var rooms = await _context.ApartmentRoom.FirstOrDefaultAsync(a => a.ApartmentId == apartment.Id);
            return View(await _context.ApartmentRoom.Include(p => p.Apartment).Where(a => a.ApartmentId == apartment).ToListAsync());
        }

        // GET: ApartmentRooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apartmentRoom = await _context.ApartmentRoom
                .FirstOrDefaultAsync(m => m.Id == id);
            if (apartmentRoom == null)
            {
                return NotFound();
            }

            return View(apartmentRoom);
        }

        // GET: ApartmentRooms/Create
        public IActionResult Create()
        {
            var apartments = ApartmentsList();
            var values = from ap in apartments
                         select ap.Text;
            ViewBag.Apartaments = values;
            return View();
        }

        // POST: ApartmentRooms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string apartamentTitle, ApartmentRoom apartmentRoom)
        {
                var apartament = _context.Apartment.SingleOrDefault(x => x.Title == apartamentTitle);
                apartmentRoom.ApartmentId = apartament.Id;
                _context.Add(apartmentRoom);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { apartment = apartmentRoom.ApartmentId });
            
        }

        // GET: ApartmentRooms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var apartmentRoom = await _context.ApartmentRoom.FindAsync(id);
            var apartment = await _context.Apartment.FindAsync(apartmentRoom.ApartmentId);
            apartmentRoom.Apartment = apartment;
            if (apartmentRoom == null)
            {
                return NotFound();
            }
            var apartments = ApartmentsList();
            var values = from ap in apartments
                         select ap.Text;
            ViewBag.Apartaments = values;
            return View(apartmentRoom);
        }

        // POST: ApartmentRooms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string apartmentTitle, ApartmentRoom apartmentRoom)
        {
            
                try
                {
                    var apartment = _context.Apartment.SingleOrDefault(x => x.Title == apartmentTitle);
                    apartmentRoom.ApartmentId = apartment.Id;
                    _context.Update(apartmentRoom);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApartmentRoomExists(apartmentRoom.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { apartment = apartmentRoom.ApartmentId });      
        }

        // GET: ApartmentRooms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apartmentRoom = await _context.ApartmentRoom
                .FirstOrDefaultAsync(m => m.Id == id);
            var apartment = _context.Apartment.SingleOrDefault(x => x.Id == apartmentRoom.ApartmentId);
            apartmentRoom.Apartment = apartment;
            if (apartmentRoom == null)
            {
                return NotFound();
            }

            return View(apartmentRoom);
        }

        // POST: ApartmentRooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var apartmentRoom = await _context.ApartmentRoom.FindAsync(id);
            _context.ApartmentRoom.Remove(apartmentRoom);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { apartment = apartmentRoom.ApartmentId });
        }

        private bool ApartmentRoomExists(int id)
        {
            return _context.ApartmentRoom.Any(e => e.Id == id);
        }

        public List<SelectListItem> ApartmentsList()
        {
            List<SelectListItem> listItems = new List<SelectListItem>();
            var apartments = _context.Apartment.ToList();
            foreach (var apartment in apartments)
            {
                listItems.Add(new SelectListItem { Text = apartment.Title, Value = apartment.Title });
            }
            return listItems;
        }

    }
}
