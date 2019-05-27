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

namespace WebApplication.Controllers
{
    public class TripController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IUserService _us;
        private User user;
        private static List<int> list;
        public TripController(ApplicationDbContext context, IUserService us)
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
            ViewBag.offices = _context.Office.ToList();
            ViewBag.users = _context.Users.ToList();
            ViewBag.tp = _context.TripParticipators.ToList();
        }
        [HttpPost]
        public JsonResult AddParticipant(int id)
        {
            if (list == null)
            {
                list = new List<int>();
            }
            Console.WriteLine(id);
            list.Add(id);
            ViewBag.list = list;
            var status = true;

            return new JsonResult(status);
        }
        [HttpPost]
        public JsonResult RemoveParticipant(int id)
        {
            Console.WriteLine(id);
            list.Remove(id);
            ViewBag.list = list;
            var status = true;

            return new JsonResult(status);
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
        // GET: Trip
        public async Task<IActionResult> Index()
        {
            return View(await _context.Trip.ToListAsync());
        }

        // GET: Trip/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trip = await _context.Trip.FirstOrDefaultAsync(m => m.Id == id);
            if (trip == null)
            {
                return NotFound();
            }
            List<User> list1 = _context.Users.ToList();
            List<TripParticipator>list2 = _context.TripParticipators.Where(x => x.TripId == trip.Id).ToList();
            List<User> list3 = new List<User>();
            foreach (TripParticipator tp in list2)
            {
                foreach (User u in list1)
                {
                    if (tp.UserId == u.Id)
                    {
                        list3.Add(u);
                    }
                }
            }
            ViewBag.Participators=list3;
            return View(trip);
        }

        // GET: Trip/Create
        public IActionResult Create()
        {
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
        public async Task<IActionResult> Create(string officeTitle1, string officeTitle2, Trip trip)
        {

            var office1 = _context.Office.SingleOrDefault(x => x.Name == officeTitle1);
            var office2 = _context.Office.SingleOrDefault(x => x.Name == officeTitle2);
            trip.ToOffice = office2.Id;
            trip.FromOffice = office1.Id;
            trip.TripStatus = 0;
            _context.Add(trip);
            await _context.SaveChangesAsync();
            Console.WriteLine(trip.Id);

            trip.Participators = new List<TripParticipator>();
            int i1 = 0;
            list = list.Distinct().ToList();
            foreach (int i in list)
            {

                Console.WriteLine("cia musus elementas " + i);
                TripParticipator participator = new TripParticipator();
                participator.TripId = trip.Id;
                participator.UserId = i;
                participator.Approve = false;
                if (i1 != i) { 
                _context.Add(participator);
                }
                i1 = i;
                
            }
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
            
            //return View(trip);
        }
        public string GetOfficeName(int id)
        {
            var name = _context.Office.SingleOrDefault(x => x.Id == id).Name;
            return name;
        }
        // GET: Trip/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trip = await _context.Trip.FindAsync(id);
            if (trip == null)
            {
                return NotFound();
            }
            return View(trip);
        }

        // POST: Trip/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Start,End,FromOffice,ToOffice,TripStatus")] Trip trip)
        {
            if (id != trip.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
            return View(trip);
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
        public IActionResult AddTripView()
        {
            return View();
        }
    }
}
