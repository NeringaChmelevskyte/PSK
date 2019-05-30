using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Application;
using Application.Entities;
using Application.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Application.Entities
{

    public enum AccomodationStatusEnum
    {
        NotRequired,
        Required,
        Booked
    }

    public class AccomodationInfo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TripId { get; set; }

        [Required]
        public int UserId { get; set; }

        public int ApartmentRoomId { get; set; }

        [Required]
        public DateTime Start { get; set; }

        [Required]
        public DateTime End { get; set; }

        public string HotelName { get; set; }

        public Single Cost { get; set; }

        public AccomodationStatusEnum AccomodationStatus { get; set; }

    }
}