using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Application;
using Application.Entities;

namespace WebApplication.Controllers
{
    public class ApartmentRoomsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ApartmentRoomsController(ApplicationDbContext context)
        {
            _context = context;
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
            return View();
        }

        // POST: ApartmentRooms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RoomNumber,ApartmentId")] ApartmentRoom apartmentRoom)
        {
            if (ModelState.IsValid)
            {
                _context.Add(apartmentRoom);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { apartment = apartmentRoom.ApartmentId });
            }
            return View(apartmentRoom);
        }

        // GET: ApartmentRooms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apartmentRoom = await _context.ApartmentRoom.FindAsync(id);
            if (apartmentRoom == null)
            {
                return NotFound();
            }
            return View(apartmentRoom);
        }

        // POST: ApartmentRooms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RoomNumber,ApartmentId")] ApartmentRoom apartmentRoom)
        {
            if (id != apartmentRoom.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
            return View(apartmentRoom);
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
    }
}
