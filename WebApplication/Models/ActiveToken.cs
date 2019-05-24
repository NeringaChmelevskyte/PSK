using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class ActiveToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public int UserId { get; set; }
    }
}
