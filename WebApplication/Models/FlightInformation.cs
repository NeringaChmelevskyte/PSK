using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public Single Cost { get; set; }

        [Required]
        public DateTime Start { get; set; }

        [Required]
        public DateTime End { get; set; }
        [DisplayName("Flight tickets status")]
        public TicketStatusEnum FlightTicketStatus { get; set; }

    }
}
