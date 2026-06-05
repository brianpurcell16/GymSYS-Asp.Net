using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GymApp.Models;

namespace GymApp.Services
{
    public interface IBookingService
    {
        BookingResult BookClass(string fname, string lname, int classId);
        void CancelBooking(int bookingId);
        List<Booking> GetActiveBookings();
        Booking GetBookingById(int bookingId);
        List<(string Month, decimal Total)> GetRevenue(int year);
    }
}