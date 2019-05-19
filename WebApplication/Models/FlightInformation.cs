using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Entities
{
    public enum TicketStatusEnum
    {
        NotRequired,
        Required,
        Booked
    }

    public class FlightInformation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TripId { get; set; }

        [Required]
        public decimal Cost { get; set; }

        [Required]
        public DateTime Start { get; set; }

        [Required]
        public DateTime End { get; set; }

        public TicketStatusEnum TicketStatus { get; set; }

    }
}
