using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Entities
{
    public enum CarRentalEnum
    {
        NotRequired,
        Required,
        Booked
    }

    public class RentalCarInformation
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
        [DisplayName("Car rent status")]
        public CarRentalEnum CarRental { get; set; }

    }
}
