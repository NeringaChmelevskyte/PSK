using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Application.Entities
{
    public enum Roles
    {
        Admin,
        Employee,
        Organizer       
    }

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
        public Roles Role { get; set; }
        public virtual ICollection<TripParticipator> Trips { get; set; }
    }
}
