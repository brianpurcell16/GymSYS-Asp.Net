using GymApp.Models;
using GymApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication.ExtendedProtection.Configuration;
using System.Web;

namespace GymApp.Services
{
    //Enum is needed because book class has five options that might be returned
    public enum BookingResult
    {
        Success,
        ClassFull,
        MemberNotFound,
        ClassNotFound,
        InsufficientFunds
    }

    public class BookingService : IBookingService
    {

        private readonly IMemberRepository _memberRepository;
        private readonly IClassRepository _classRepository;
        private readonly IBookingRepository _bookingRepository;

        public BookingService(IMemberRepository memberRepository, IClassRepository classRepository, IBookingRepository bookingRepository)
        {
            _memberRepository = memberRepository;
            _classRepository = classRepository;
            _bookingRepository = bookingRepository;
        }

        public BookingResult BookClass(string fname, string sname, int classId)
        {
            var member = _memberRepository.GetByName(fname, sname);
            if (member == null)
                return BookingResult.MemberNotFound;

            var gymClass = _classRepository.GetById(classId);
            if (gymClass == null || gymClass.Status != "A")
                return BookingResult.ClassNotFound;

            if (!_classRepository.DecreaseSpaces(classId))
                return BookingResult.ClassFull;

            if (!_bookingRepository.DeductWallet(fname, sname, classId))
            {
                _classRepository.IncreaseSpaces(classId); // Revert space decrease if payment fails
                return BookingResult.InsufficientFunds;
            }

            _bookingRepository.CreateBooking(member.MemID, classId, gymClass.Price);

            return BookingResult.Success;
        }

        public void CancelBooking(int bookingId)
        {
            _bookingRepository.CancelBooking(bookingId);
        }

        public List<Booking> GetActiveBookings()
        {
            return _bookingRepository.GetActiveBookings();
        }

        public Booking GetBookingById(int bookingId)
        {
            return _bookingRepository.GetById(bookingId);
        }

        public List<(string Month, decimal Total)> GetRevenue(int year)
        {
            return _bookingRepository.GetRevenueByYear(year);
        }
    }
}