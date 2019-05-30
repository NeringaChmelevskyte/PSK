using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace Application.Entities
{

    public class Log
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string Text { get; set; }
    }
}
