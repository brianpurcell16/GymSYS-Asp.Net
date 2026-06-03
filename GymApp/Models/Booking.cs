using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GymApp.Models
{
    public class Booking
    {
        public int BookingID { get; set; }
        public int MemID { get; set; }
        public int ClassID { get; set; }
        public decimal Price { get; set; }
        public DateTime DateBooked { get; set; }

        public string Status { get; set; }
    }
}