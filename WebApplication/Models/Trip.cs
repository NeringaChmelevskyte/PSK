using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [ForeignKey("Office")]
        public int FromOffice { get; set; }

        [Required]
        [ForeignKey("Office2")]
        public int ToOffice { get; set; }

        [Required]
        public TripStatusEnum TripStatus { get; set; }

        public virtual ICollection<TripParticipator> Participators { get; set; }
        public virtual Office Office { get; set; }
        public virtual Office Office2 { get; set; }




    }
}
