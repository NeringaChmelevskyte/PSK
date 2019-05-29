using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Entities
{
    public class TripParticipator
    {
        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        [ForeignKey("Trip")]
        public int TripId { get; set; }
        public Trip Trip { get; set; }

        public bool Approve{ get; set; }
    }
}
