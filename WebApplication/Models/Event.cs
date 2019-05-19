using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Entities
{
    public enum BookingTypeEnum
    {
        UserEvent,
        UserTrip
    }

    public class Event
    {
        [Key]
        public int EventId { get; set; }
        [Required]
        public string Subject { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "")]
        public string Description { get; set; }
        [Required]
        public DateTime Start { get; set; }
        [Required]
        public DateTime End { get; set; }
        [Required]
        public string ThemeColor { get; set; }
        [Required]
        public bool IsFullDay { get; set; }
        [Required]
        public int UserId { get; set; }


    }
}
