using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GymApp.Models;


namespace GymApp.Repositories
{
    public interface IBookingRepository
    {
        void CreateBooking(int  memId, int classId, decimal price);
        Booking GetById(int bookingId);
        List<Booking> GetActiveBookings();
        void CancelBooking(int bookingId);
        bool DeductWallet(string fname, string lname, int classId);
        List<(string Month, decimal Total)> GetRevenueByYear(int year);
    }
}