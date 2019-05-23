using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Entities
{
    public enum TripStatusEnum
    {
        WaitingForApproval,
        Approved,
        InProgress,
        Completed,
        Canceled
    }

    public class Trip
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public DateTime Start { get; set; }

        [Required]
        public DateTime End { get; set; }

        [Required]
        public int FromOffice { get; set; }

        [Required]
        public int ToOffice { get; set; }

        [Required]
        public TripStatusEnum TripStatus { get; set; }

        public virtual ICollection<TripParticipator> Participators { get; set; }
    }
}
