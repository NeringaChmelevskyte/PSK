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
            
            var tempFlightInformation = _context.FlightInformation.SingleOrDefault(f => f.TripId == flightInformation.TripId);

            tempFlightInformation.Start = flightInformation.Start;
            tempFlightInformation.Details = flightInformation.Details;
            tempFlightInformation.End = flightInformation.End;
            tempFlightInformation.Cost = flightInformation.Cost;
            tempFlightInformation.FlightTicketStatus = flightInformation.FlightTicketStatus;

            _context.Update(tempFlightInformation);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Trip", new { id = tempFlightInformation.TripId});

        }

    }
}