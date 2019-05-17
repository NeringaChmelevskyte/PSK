using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.Entities
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [DisplayName("Lastname")]
        [Required]
        public string Surname { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
    }
}
