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
    public class RentalCarInformationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IUserService _us;
        private User user;

        public RentalCarInformationController(ApplicationDbContext context, IUserService us)
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
            var tmpRentalCarInformation = _context.RentalCarInformation.FirstOrDefault(r => r.TripId == id);

            return View(tmpRentalCarInformation);
        }

        public IActionResult Create(int id)
        {
            var tmpRentalCarInformation = _context.RentalCarInformation.FirstOrDefault(r => r.TripId == id);

            return View(tmpRentalCarInformation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RentalCarInformation rentalCarInformation)
        {
           var tempRentalCarInformation = _context.RentalCarInformation.SingleOrDefault(r => r.TripId == rentalCarInformation.TripId);

            tempRentalCarInformation.Start = rentalCarInformation.Start;
            tempRentalCarInformation.End = rentalCarInformation.End;
            tempRentalCarInformation.Cost = rentalCarInformation.Cost;
            tempRentalCarInformation.CarRental = rentalCarInformation.CarRental;
            tempRentalCarInformation.Name = rentalCarInformation.Name;

            _context.Update(tempRentalCarInformation);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Trip", new { id = tempRentalCarInformation.TripId });


        }

    }
}