using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GymApp.Services;
using GymApp.Models;

namespace GymApp.Controllers
{
    public class BookingController : Controller
    {

        private readonly IBookingService _bookingService;
        private readonly IClassService _classService;
        public BookingController(IBookingService bookingService, IClassService classService)
        {
            _bookingService = bookingService;
            _classService = classService;
        }

        // GET: Booking/Book
        public ActionResult Book()
        {
            var activeClasses = _classService.GetActiveClasses();
            return View(activeClasses);
        }

        // POST: Booking/Book
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Book(string fname, string sname, int classId)
        {
            var result = _bookingService.BookClass(fname, sname, classId);

            switch (result)
            {
                case BookingResult.Success:
                    TempData["Success"] = "Booking has been confirmed!";
                    return RedirectToAction("Book");

                case BookingResult.ClassFull:
                    ModelState.AddModelError("", "Sorry, this class is full.");
                    break;

                case BookingResult.MemberNotFound:
                    ModelState.AddModelError("", "Member not found. Please check the name and try again.");
                    break;

                case BookingResult.ClassNotFound:
                    ModelState.AddModelError("", "Class not found. Please check the class ID and try again.");
                    break;

                case BookingResult.InsufficientFunds:
                    ModelState.AddModelError("", "Insufficient funds in wallet. Please top up and try again.");
                    break;
            }

            //Reloading classes here incases something goes wrong but it shouldnt reach here if the booking was successful
            var activeClasses = _classService.GetActiveClasses();
            return View(activeClasses);
        }

        //GET: Booking/Cancel
        public ActionResult Cancel()
        {
            var activeBookings = _bookingService.GetActiveBookings();
            return View(activeBookings);
        }

        //POST: Booking/Cancel
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cancel(int bookingId)
        {
            // the stored procedure will handle refunding the wallet, increasing the class spaces and making the booking inactive
            _bookingService.CancelBooking(bookingId);
            TempData["Success"] = "Booking has been cancelled.";
            return RedirectToAction("Cancel");
        }

        //GET: Booking/Revenue
        public ActionResult Revenue()
        {
            return View(new List<(string Month, decimal Total)>());
        }

        //POST: Booking/Revenue
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Revenue(int year)
        {
            var revenueData = _bookingService.GetRevenue(year);
            return View(revenueData);
        }
    }

}