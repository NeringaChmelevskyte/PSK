using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Entities
{
    public class ApartmentRoom
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RoomNumber { get; set; }

        [Required]
        public int ApartmentId { get; set; }
    }
}
