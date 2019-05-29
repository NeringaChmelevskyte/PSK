using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Application.IServices;
using Application.Entities;
using System.Security.Cryptography;
using WebApplication.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using Application.Entities;

namespace Application.Services
{
    public class TripService : ITripService
    {
        protected readonly ApplicationDbContext _context;
        public TripService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void ApproveTripForUser(int tripId, int userId)
        {
            User user = _context.Users.Find(userId);
            Trip trip = _context.Trip.Include(x => x.Participators).Where(x => x.Id == tripId).FirstOrDefault();
            TripParticipator tp = _context.TripParticipators.Where(x => x.UserId == user.Id && x.TripId == trip.Id).FirstOrDefault();
            tp.Approve = true;
            _context.Update(tp);

            if (!trip.Participators.Any(x => x.Approve == false))
            {
                trip.TripStatus = TripStatusEnum.Approved;
                _context.Update(trip);
            }
            _context.SaveChanges();
        }

        public void DeclineTripForUser(int tripId, int userId)
        {
            TripParticipator tp = _context.TripParticipators.Where(x => x.UserId == userId && x.TripId == tripId).FirstOrDefault();
            _context.Remove(tp);
            _context.SaveChanges();
            Trip trip = _context.Trip.Include(x => x.Participators).Where(x => x.Id == tripId).FirstOrDefault();
            if(trip.Participators == null || trip.Participators.Count == 0)
            {
                trip.TripStatus = TripStatusEnum.Canceled;
                _context.Update(trip);
            }
            _context.SaveChanges();
        }

        private bool DateIsBetween(DateTime date, DateTime first, DateTime second)
        {
            if (date > first && date < second) return true;
            else return false;
        }

        public bool IsTripParticipatorsBusy(Trip trip)
        {
            bool busy = false;
            foreach(TripParticipator human in trip.Participators)
            {
                var user = _context.Users.Where(i => i.Id == human.UserId).FirstOrDefault();
                List<Event> userEvents = _context.Events.Where(x => x.UserId == user.Id && x.Start > DateTime.Now).ToList();
                if (userEvents.Any(x => DateIsBetween(x.Start, trip.Start, trip.End) || DateIsBetween(x.End, trip.Start, trip.End))) busy = true;
            }
            return busy;
        }
    }
}
