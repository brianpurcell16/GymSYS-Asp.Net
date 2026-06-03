using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GymApp.Models
{
    public class Booking
    {
        public int BookingId { get; set; }
        public int MemId { get; set; }
        public int ClassId { get; set; }
        public decimal Price { get; set; }
        public DateTime DateBooked { get; set; }

        public string Status { get; set; }
    }
}