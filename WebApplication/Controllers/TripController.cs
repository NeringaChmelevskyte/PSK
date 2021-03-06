﻿using System;
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
        private ITripService _ts;
        private User user;
        private static List<int> list;
        private static int id1;
        private static int id2;
        public TripController(ApplicationDbContext context, IUserService us, ITripService ts)
        {
            _context = context;
            _us = us;
            _ts = ts;
        }
        public override void OnActionExecuting(ActionExecutingContext ctx)
        {
            base.OnActionExecuting(ctx);
            user = _us.GetUserFromRequest(Request);
            if (user == null)
            {
                ViewBag.Name = "";
                ViewBag.Role = null;
            }

            else
            {
                ViewBag.Name = user.Name + " " + user.Surname;
                ViewBag.Role = user.Role;
                ViewBag.Id = user.Id;
            }
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
            var user = _us.GetUserFromRequest(Request);
            if (user != null && ViewBag.Role == Roles.Admin || ViewBag.Role == Roles.Organizer)
            { return View(await _context.Trip.ToListAsync()); }
            else { return View("_NotFound"); }
        }

        // GET: Trip/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _context.FlightInformation.FirstOrDefaultAsync(f => f.TripId == id);
            var carRental = await _context.RentalCarInformation.FirstOrDefaultAsync(r => r.TripId == id);
            var accomodationInfo = await _context.AccomodationInfo.FirstOrDefaultAsync(a => a.TripId == id);

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
            //ViewBag.FlightId = flight.Id;
            ViewBag.FlightTicketStatus = flight.FlightTicketStatus;


            //ViewBag.CarRentalId = carRental.Id;
            ViewBag.CarRental = carRental.CarRental;

            ViewBag.AccomodationStatus = accomodationInfo.AccomodationStatus;

            if (user != null && ViewBag.Role == Roles.Admin || ViewBag.Role == Roles.Organizer) { 
            return View(trip); 
            }
            else { return View("_NotFound"); }

        }
        public async Task<IActionResult> DetailsView(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _context.FlightInformation.FirstOrDefaultAsync(f => f.TripId == id);
            var carRental = await _context.RentalCarInformation.FirstOrDefaultAsync(r => r.TripId == id);
            var accomodationInfo = await _context.AccomodationInfo.FirstOrDefaultAsync(a => a.TripId == id);

            var trip = await _context.Trip.FirstOrDefaultAsync(m => m.Id == id);
            if (trip == null)
            {
                return NotFound();
            }
            //ViewBag.FlightId = flight.Id;
            ViewBag.FlightTicketStatus = flight.FlightTicketStatus;


            //ViewBag.CarRentalId = carRental.Id;
            ViewBag.CarRental = carRental.CarRental;

            ViewBag.AccomodationStatus = accomodationInfo.AccomodationStatus;

            if (user != null)
            {
                return View(trip);
            }
            else { return View("_NotFound"); }

        }
        public async Task<IActionResult> AddInfo(int? id, int? uid)
        {
            var tripParticipator = await  _context.TripParticipators.FirstOrDefaultAsync(m => m.TripId == id && m.UserId == uid);
            ViewBag.id = id;
            ViewBag.uid = uid;
            id1 = (int)id;
            id2 = (int)uid;
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddInfo(TripParticipator tp)
        {
            var tripParticipator = await _context.TripParticipators.FirstOrDefaultAsync(m => m.TripId == id1 && m.UserId==id2);
            tripParticipator.Info = tp.Info;
            _context.Update(tripParticipator);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = id1 });
        }
        [HttpPost]
        public IActionResult Index(string firstName)
        {
            return Content($"Hello {firstName}");
        }

        // GET: Trip/Create
        public IActionResult Create(string error = "")
        {
            list = null;
            var offices = OfficeList();
            var values = from ofc in offices
                         select ofc.Text;
            ViewBag.Offices = values;
            ViewBag.FlightTicketStatus = new List<TicketStatusEnum>() { TicketStatusEnum.Required, TicketStatusEnum.NotRequired};
            ViewBag.carRental = new List<CarRentalEnum>() { CarRentalEnum.Required, CarRentalEnum.NotRequired };
            ViewBag.AccomodationStatus = new List<AccomodationStatusEnum> { AccomodationStatusEnum.Required, AccomodationStatusEnum.NotRequired };
            ViewBag.Error = error;
            if (user != null && ViewBag.Role == Roles.Admin || ViewBag.Role == Roles.Organizer)
            { return View(); }
            else { return View("_NotFound"); }
        }

        // POST: Trip/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string officeTitle1, string officeTitle2, Trip trip, TicketStatusEnum ticketStatus, CarRentalEnum carRental, AccomodationStatusEnum accomodationStatus)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var office1 = _context.Office.SingleOrDefault(x => x.Name == officeTitle1);
                    var office2 = _context.Office.SingleOrDefault(x => x.Name == officeTitle2);
                    trip.ToOffice = office2.Id;
                    trip.FromOffice = office1.Id;
                    trip.TripStatus = 0;

                    user = _us.GetUserFromRequest(Request);
                    trip.Organizator = user.Id;
                    _context.Add(trip);
                    //await _context.SaveChangesAsync();
                    Console.WriteLine(trip.Id);


                    trip.Participators = new List<TripParticipator>();
                    int i1 = 0;
                    if (list == null || list.Count() == 0)
                    {
                        return RedirectToAction(nameof(Create), new { error = "Kelionėje turi dalyvauti bent vienas dalyvis" });
                    }
                    list = list.Distinct().ToList();
                    foreach (int i in list)
                    {
                        TripParticipator participator = new TripParticipator();
                        participator.TripId = trip.Id;
                        participator.UserId = i;
                        participator.Approve = false;
                        if (i1 != i)
                        {
                            trip.Participators.Add(participator);
                        }
                        i1 = i;

                    }
                    if (!_ts.IsTripParticipatorsBusy(trip))
                    {
                        _context.Add(trip);
                        _context.SaveChanges();

                        foreach (TripParticipator tripParticipator in trip.Participators)
                        {
                            var accomodationInfo = new AccomodationInfo() { TripId = trip.Id, UserId = tripParticipator.UserId, Start = DateTime.MinValue, End = DateTime.MinValue, AccomodationStatus = accomodationStatus };
                            _context.Add(accomodationInfo);
                        }

                        var flightInfo = new FlightInformation() { TripId = trip.Id, Cost = 0, Start = DateTime.MinValue, End = DateTime.MinValue, FlightTicketStatus = ticketStatus };
                        _context.Add(flightInfo);
                        var rentalCarinfo = new RentalCarInformation() { TripId = trip.Id, Cost = 0, Start = DateTime.MinValue, End = DateTime.MinValue, CarRental = carRental };
                        _context.Add(rentalCarinfo);

                        _context.SaveChanges();
                        transaction.Commit();
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        return RedirectToAction(nameof(Create), new { error = "Pasirinkti dalyiai šiuo laiku užimti" });
                    }
                }
                catch (Exception)
                {
                    return NotFound();
                }
            }
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
            ViewBag.DefaultOfficeFrom = _context.Office.Find(trip.FromOffice).Name;
            ViewBag.DefaultOfficeTo = _context.Office.Find(trip.ToOffice).Name;
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
            ViewBag.FlightTicketStatus = new List<TicketStatusEnum>() { TicketStatusEnum.Required, TicketStatusEnum.NotRequired, TicketStatusEnum.Booked };
            ViewBag.DefaultFlightTicketStatus = _context.FlightInformation.First(a => a.TripId == trip.Id).FlightTicketStatus;
            ViewBag.RentalCarStatus = new List<CarRentalEnum>() { CarRentalEnum.Required, CarRentalEnum.NotRequired, CarRentalEnum.Booked  };
            ViewBag.DefaultRentalCarStatus = _context.RentalCarInformation.First(a => a.TripId == trip.Id).CarRental;
            ViewBag.AccomodationStatus = new List<AccomodationStatusEnum>() { AccomodationStatusEnum.Required, AccomodationStatusEnum.NotRequired, AccomodationStatusEnum.Booked };
            ViewBag.DefaultAccomodationStatus = _context.AccomodationInfo.First(a => a.TripId == trip.Id).AccomodationStatus;
            
            var user = _us.GetUserFromRequest(Request);
            if (user != null && ViewBag.Role == Roles.Admin || ViewBag.Role == Roles.Organizer) { return View(trip); }
            else { return View("_NotFound"); }
        }

        // POST: Trip/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string fromOffice, string toOffice, Trip trip, TicketStatusEnum ticketStatus, CarRentalEnum rentalCarStatus, AccomodationStatusEnum accomodationStatus)
        {
            
                try
                {
                var office1 = _context.Office.SingleOrDefault(x => x.Name == fromOffice);
                var office2 = _context.Office.SingleOrDefault(x => x.Name == toOffice);
                trip.ToOffice = office2.Id;
                trip.FromOffice = office1.Id;

                user = _us.GetUserFromRequest(Request);
                trip.Organizator = user.Id;
                _context.Update(trip);
                await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException a)
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
            var flightInfo = _context.FlightInformation.Single(a => a.TripId == trip.Id);
            flightInfo.FlightTicketStatus = ticketStatus;

            var rentalCar = _context.RentalCarInformation.Single(a => a.TripId == trip.Id);
            rentalCar.CarRental = rentalCarStatus;

            var accomodationList = _context.AccomodationInfo.Where(a => a.TripId == trip.Id).ToList();

            for(int i =0; i < accomodationList.Count ; i++)
            {
                var tempAccomodation = accomodationList[i];
                tempAccomodation.AccomodationStatus = accomodationStatus;
            }
            


            await _context.SaveChangesAsync();

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
            trip.Office = officeFrom;
            var officeTo = await _context.Office.FindAsync(trip.ToOffice);
            trip.Office2 = officeTo;
            if (trip == null)
            {
                return NotFound();
            }

            if (user != null && ViewBag.Role == Roles.Admin || ViewBag.Role == Roles.Organizer) { return View(trip); }
            else { return View("_NotFound"); }
        }

        // POST: Trip/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trip = await _context.Trip.FindAsync(id);
            var flightInformation = _context.FlightInformation.Single(f => f.TripId == id);
            var rentalCarInformation = _context.RentalCarInformation.Single(r => r.TripId == id);
            var accomodationInfo = _context.AccomodationInfo.Where(a => a.TripId == id).ToList();

            _context.Trip.Remove(trip);
            _context.FlightInformation.Remove(flightInformation);
            _context.RentalCarInformation.Remove(rentalCarInformation);

            foreach( AccomodationInfo accomodation in accomodationInfo)
            {
                _context.AccomodationInfo.Remove(accomodation);
            }

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
            var user = _us.GetUserFromRequest(Request);
            if (user != null && ViewBag.Role == Roles.Admin || ViewBag.Role == Roles.Organizer) { return View(); }
            else { return View("_NotFound"); }
        }

        [HttpPost]
        public async Task<ActionResult> Merge(int[] mergeInputs)
        {

            if (mergeInputs == null)
            {

                ModelState.AddModelError("", "None of the records has been selected for merge action !");
                return RedirectToAction(nameof(Index));
            }
            List<Trip> trips = new List<Trip>();
            var i = 0;
            foreach (var item in mergeInputs)
            {
                   trips.Add(await _context.Trip.FindAsync(item));
                   trips[i].Office = _context.Office.SingleOrDefault(x => x.Id == trips[i].FromOffice);
                   trips[i].Office2 = _context.Office.SingleOrDefault(x => x.Id == trips[i].ToOffice);

                    i++;
            }
            TempData ["trips"] = trips; 
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
            return View(trips.FirstOrDefault());
        }

        [HttpPost]
        public async Task<ActionResult> MergeSelectedTrips(string fromOffice, string toOffice, int[] trips, Trip trip)
        {
            var office1 = _context.Office.SingleOrDefault(x => x.Name == fromOffice);
            var office2 = _context.Office.SingleOrDefault(x => x.Name == toOffice);
            trip.ToOffice = office2.Id;
            trip.FromOffice = office1.Id;
            user = _us.GetUserFromRequest(Request);
            trip.Organizator = user.Id;
            _context.Add(trip);

            List<TripParticipator> participators = new List<TripParticipator>();

            foreach (var i in trips)
            {
                List<TripParticipator> list2 = _context.TripParticipators.Where(x => x.TripId == i).ToList();
                var removeTrip = await _context.Trip.FindAsync(i);

                List<AccomodationInfo> accomToDelete = new List<AccomodationInfo>();

                _context.FlightInformation.Remove(_context.FlightInformation.Single(f => f.TripId == i));
                _context.RentalCarInformation.Remove(_context.RentalCarInformation.Single(r => r.TripId == i));

                foreach (var p in list2)
                {
                    TripParticipator tp = new TripParticipator();
                    tp.TripId = trip.Id;
                    tp.Trip = trip;
                    tp.UserId = p.UserId;
                    tp.User = p.User;
                    _context.Remove(p);
                    TripParticipator trPr = await _context.TripParticipators.FindAsync(tp.TripId, tp.UserId);
                    if (trPr == null)
                    {
                        
                        //var tmpAccom = _context.AccomodationInfo.Single(a => a.TripId == i);
                            participators.Add(p);
                            _context.Add(tp);


                    }
                    var accomInfo = _context.AccomodationInfo.Single(a => a.TripId == i && a.UserId == p.UserId);
                    _context.AccomodationInfo.Remove(accomInfo);
                }
                
                _context.Remove(removeTrip);
                await _context.SaveChangesAsync();

            }

            //var accomInfo = _context.AccomodationInfo.Single(a => a.TripId == 7);

            trip.Participators = participators;
            await _context.SaveChangesAsync();

            int sk = 0;
            foreach (TripParticipator tp in trip.Participators)
            {
                _context.AccomodationInfo.Add(new AccomodationInfo { AccomodationStatus = AccomodationStatusEnum.NotRequired, TripId = tp.TripId, UserId = tp.UserId });
                if(sk == 0)
                {
                    _context.Add(new FlightInformation { FlightTicketStatus = TicketStatusEnum.NotRequired, TripId = trip.Id });
                    _context.Add(new RentalCarInformation { CarRental = CarRentalEnum.NotRequired, TripId = trip.Id });
                    sk++;
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
