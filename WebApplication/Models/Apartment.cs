using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Entities
{
    public class Apartment
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public int OfficeId { get; set; }

        [Required]
        [DisplayName("Room count")]
        public int RoomCount { get; set; }

        public virtual Office Office { get; set; }
    }
}
