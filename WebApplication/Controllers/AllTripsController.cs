using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Application;
using Application.Entities;
using Application.IServices;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace WebApplication.Controllers
{
    public class AllTripsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IUserService _us;
        private User user;

        public AllTripsController(ApplicationDbContext context, IUserService us)
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
        // GET: Trip
        public async Task<IActionResult> Index()
        {
             var trips = await _context.Trip.Include(p => p.Office).Include(o => o.Office2).ToListAsync();
             return View(trips);
        }
        

        // GET: Trip/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trip = await _context.Trip.Include(p => p.Office).Include(o => o.Office2)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trip == null)
            {
                return NotFound();
            }

            return View(trip);
        }

        // GET: Trip/Create
        public IActionResult Create()
        {
            var enumData = from TripStatusEnum t in Enum.GetValues(typeof(TripStatusEnum))
                           select new
                           {
                               ID = (int)t,
                               Name = t.ToString()
                           };
            ViewBag.EnumList = new SelectList(enumData, "ID", "Name");
            var offices = OfficeList();
            var values = from ofc in offices
                         select ofc.Text;
            ViewBag.Offices = values;
            return View();
        }

        // POST: Trip/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string fromOffice, string toOffice, Trip trip)
        {
            var officeFrom = _context.Office.SingleOrDefault(x => x.Name == fromOffice);
            trip.FromOffice = officeFrom.Id;
            var officeTo = _context.Office.SingleOrDefault(x => x.Name == toOffice);
            trip.ToOffice = officeTo.Id;
            _context.Add(trip);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            
        }

        // GET: Trip/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trip = await _context.Trip.FindAsync(id);
            var officeFrom = await _context.Office.FindAsync(trip.FromOffice);
            trip.FromOffice = officeFrom.Id;
            var officeTo = await _context.Office.FindAsync(trip.ToOffice);
            trip.ToOffice = officeTo.Id;
            if (trip == null)
            {
                return NotFound();
            }
            var enumData = from TripStatusEnum t in Enum.GetValues(typeof(TripStatusEnum))
                           select new
                           {
                               ID = (int)t,
                               Name = t.ToString()
                           };
            ViewBag.EnumList = new SelectList(enumData, "ID", "Name");
            var offices = OfficeList();
            var values = from ofc in offices
                         select ofc.Text;
            ViewBag.Offices = values;
            return View(trip);
        }

        // POST: Trip/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string fromOffice, string toOffice, Trip trip)
        {
          
            try
            {
                var officeFrom = _context.Office.SingleOrDefault(x => x.Name == fromOffice);
                trip.FromOffice = officeFrom.Id;
                var officeTo = _context.Office.SingleOrDefault(x => x.Name == toOffice);
                trip.ToOffice = officeTo.Id;
                _context.Update(trip);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                    if (!TripExists(trip.Id))
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

        // GET: Trip/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trip = await _context.Trip
                .FirstOrDefaultAsync(m => m.Id == id);
            var officeFrom = await _context.Office.FindAsync(trip.FromOffice);
            trip.FromOffice = officeFrom.Id;
            var officeTo = await _context.Office.FindAsync(trip.ToOffice);
            trip.ToOffice = officeTo.Id;
            if (trip == null)
            {
                return NotFound();
            }

            return View(trip);
        }

        // POST: Trip/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trip = await _context.Trip.FindAsync(id);
            _context.Trip.Remove(trip);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TripExists(int id)
        {
            return _context.Trip.Any(e => e.Id == id);
        }

        [HttpPost]
        public JsonResult AddFlight(FlightInformation flightInformation)
        {
            var status = false;
            if (flightInformation.TripId > 0)
            {

                var v = _context.FlightInformation.Where(a => a.TripId == flightInformation.TripId).FirstOrDefault();
                if (v != null)
                {

                    //v.Id = flightInformation.Id;
                    v.Id = v.Id;
                    v.TripId = flightInformation.TripId;
                    v.Cost = flightInformation.Cost;
                    v.Start = flightInformation.Start;
                    v.End = flightInformation.End;
                    v.FlightTicketStatus = flightInformation.FlightTicketStatus;
                    _context.Update(v);
                }
                else
                {
                    _context.Add(flightInformation);
                }

            }
            _context.SaveChanges();
            status = true;

            return new JsonResult(status);
        }
        [HttpGet]
        public IActionResult GetTrip(int id)
        {


            var trips = _context.Trip.ToList();
            var trip = trips.Where(t => t.Id == id).ToList();
            //var filtered_events = events.Where(x => x.UserId == id1).ToList();

            return new JsonResult(trip);

        }

        [HttpGet]
        public IActionResult GetFlight(int id)
        {


            var flights = _context.FlightInformation.ToList();
            var flight = flights.Where(t => t.TripId == id).ToList();
            //var filtered_events = events.Where(x => x.UserId == id1).ToList();

            return new JsonResult(flight);
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
