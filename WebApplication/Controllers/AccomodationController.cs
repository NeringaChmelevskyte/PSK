using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using Application.Entities;
using Application.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication.Controllers
{
    public class AccomodationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IUserService _us;
        private User user;

        public AccomodationController(ApplicationDbContext context, IUserService us)
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
            ViewBag.Users = _context.Users.ToList();
            ViewBag.Apartments = _context.Apartment.ToList();
            ViewBag.Accomodations = _context.AccomodationInfo.Where(a => a.TripId == id).ToList();
            ViewBag.ApartmentRooms = _context.ApartmentRoom.ToList();

            
            return View(id);
        }
        public IActionResult DetailsView(int? id)
        {
            ViewBag.Apartments = _context.Apartment.ToList();
            ViewBag.Accomodations = _context.AccomodationInfo.Where(a => a.TripId == id).ToList();
            ViewBag.ApartmentRooms = _context.ApartmentRoom.ToList();
            var tmpAccomodationInfo = _context.AccomodationInfo.Where(a => a.TripId == id).ToList();
            foreach(AccomodationInfo ai in tmpAccomodationInfo)
            {
                if (user.Id == ai.UserId)
                {
                    ViewBag.Accomodation = ai;
                }
            }
            return View(id);
        }

        public IActionResult Create(int id, bool? isHotelRequired, DateTime? start, DateTime? end)
        {
            var tmpAccomodationInfo = _context.AccomodationInfo.FirstOrDefault(a => a.TripId == id);
            if (start != null && end != null)
            {
                tmpAccomodationInfo.Start = (DateTime)start;
                tmpAccomodationInfo.End = (DateTime)end;
            }
            if (isHotelRequired == false)
            {
                tmpAccomodationInfo.HotelName = null;
                tmpAccomodationInfo.Cost = 0;
            }

            ViewBag.HotelRequired = isHotelRequired;
            return View(tmpAccomodationInfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AccomodationInfo accomodationInfo)
        {

            
            var accomodations = _context.AccomodationInfo.Where(a => a.TripId == accomodationInfo.TripId).ToList();

            var officeDestinationId = _context.Trip.Single(t => t.Id == accomodationInfo.TripId).ToOffice;

            var apartmentId = _context.Apartment.Single(a => a.OfficeId == officeDestinationId).Id;

            var apartmentRooms = _context.ApartmentRoom.Where(a => a.ApartmentId == apartmentId).ToList();

            var trip = _context.Trip.Single(t => t.Id == accomodationInfo.TripId);

            var tripParticipators = _context.TripParticipators.Where(t => t.TripId == trip.Id).ToList();

            List<ApartmentRoom> emptyRooms = GetAvailableRooms(apartmentRooms, accomodationInfo.Start, accomodationInfo.End);

            if (accomodationInfo.Cost != 0 || accomodationInfo.HotelName != null)
            {
                
                foreach(TripParticipator tripParticipator in tripParticipators)
                {
                    var accomodation = accomodations.Single(a => a.UserId == tripParticipator.UserId);

                    accomodation.TripId = trip.Id;
                    accomodation.UserId = tripParticipator.UserId;
                    accomodation.ApartmentRoomId = 0;
                    accomodation.Start = accomodationInfo.Start;
                    accomodation.End = accomodationInfo.End;
                    accomodation.HotelName = accomodationInfo.HotelName;
                    accomodation.Cost = accomodationInfo.Cost;
                    accomodation.AccomodationStatus = AccomodationStatusEnum.Booked;
                    
                    _context.Update(accomodation);
                }
                _context.SaveChanges();

                return RedirectToAction("Details", "Trip", new { id = accomodationInfo.TripId });
            }

            if ( tripParticipators.Count > emptyRooms.Count)
            {
                // kambariu maziau nei keliautoju, automatiskai grizt ir uzsakyt viesbuti
                ViewBag.HotelRequired = true;
                return RedirectToAction("Create", "Accomodation", new { id = accomodationInfo.Id, isHotelRequired = true, start = accomodationInfo.Start, end = accomodationInfo.End });
            }

            int i = 0;
            foreach (TripParticipator tripParticipator in tripParticipators)
            {
                var accomodation = accomodations.Single(a => a.UserId == tripParticipator.UserId);
                ApartmentRoom apRoom = emptyRooms[i];

                accomodation.TripId = trip.Id;
                accomodation.UserId = tripParticipator.UserId;
                accomodation.ApartmentRoomId = apRoom.Id;
                accomodation.HotelName = null;
                accomodation.Cost = 0;
                accomodation.Start = accomodationInfo.Start;
                accomodation.End = accomodationInfo.End;
                accomodation.AccomodationStatus = AccomodationStatusEnum.Booked;
                
                _context.Update(accomodation);

                i++;
            }

            _context.SaveChanges();

            return RedirectToAction("Details", "Trip", new { id = accomodationInfo.TripId });
            //return RedirectToAction("a");

        }

        public List<ApartmentRoom> GetAvailableRooms(List<ApartmentRoom> rooms, DateTime start, DateTime end)
        {
            List<ApartmentRoom> availableRooms = new List<ApartmentRoom>();
            foreach (ApartmentRoom room in rooms)
            {
                List<AccomodationInfo> accomodationInfo = _context.AccomodationInfo.Where(a => a.ApartmentRoomId == room.Id).ToList();

                if( accomodationInfo.Any(a => ((a.Start >= start && a.Start < end) || (a.End > start && a.End <= end))))
                {
                    continue;
                }
                availableRooms.Add(room);
            }

            return availableRooms;
        }

    }
}