using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using Application.Entities;
using Application.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApplication.Controllers
{
    public class FlightInformationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IUserService _us;
        private User user;

        public FlightInformationController(ApplicationDbContext context, IUserService us)
        {
            _context = context;
            _us = us;
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
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var flight = await _context.FlightInformation.FirstOrDefaultAsync(f => f.TripId == id);

            //var trip = await _context.Trip.FirstOrDefaultAsync(m => m.Id == id);
            //if (trip == null)
            //{
            //    return NotFound();
            //}
            //List<User> list1 = _context.Users.ToList();
            //List<TripParticipator> list2 = _context.TripParticipators.Where(x => x.TripId == trip.Id).ToList();
            //List<User> list3 = new List<User>();
            //foreach (TripParticipator tp in list2)
            //{
            //    foreach (User u in list1)
            //    {
            //        if (tp.UserId == u.Id)
            //        {
            //            list3.Add(u);
            //        }
            //    }
            //}
            //ViewBag.Participators = list3;
            //ViewBag.FlightId = flight.Id;
            //ViewBag.FlightTicketStatus = flight.FlightTicketStatus;
            var tmpFlightInformation = _context.FlightInformation.FirstOrDefault(f => f.TripId == id);

            return View(tmpFlightInformation);
        }

        public IActionResult Create(int id)
        {
            var tmpFlightInformation = _context.FlightInformation.FirstOrDefault(f => f.TripId == id);

            return View(tmpFlightInformation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FlightInformation flightInformation)
        {
            //var apartament = _context.Apartment.SingleOrDefault(x => x.Title == apartamentTitle);
            //apartmentRoom.ApartmentId = apartament.Id;
            var tempFlightInformation = _context.FlightInformation.SingleOrDefault(f => f.TripId == flightInformation.TripId);

            tempFlightInformation.Start = flightInformation.Start;
            tempFlightInformation.End = flightInformation.End;
            tempFlightInformation.Cost = flightInformation.Cost;
            tempFlightInformation.FlightTicketStatus = flightInformation.FlightTicketStatus;

            _context.Update(tempFlightInformation);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Trip", new { id = tempFlightInformation.TripId});
            //return RedirectToAction("a");

        }

    }
}