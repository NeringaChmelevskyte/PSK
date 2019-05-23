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
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        public int TripId { get; set; }
        public Trip Trip { get; set; }
    }
}
