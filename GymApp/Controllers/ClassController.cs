using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GymApp.Services;
using GymApp.Models;

namespace GymApp.Controllers
{
    public class ClassController : Controller
    {
        private readonly IClassService _classService;

        public ClassController(IClassService classService)
        {
            _classService = classService;
        }

        // GET: Class/Schedule
        public ActionResult Schedule()
        {
            return View();
        }

        // POST: Class/Schedule
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Schedule(GymClass gymClass)
        {
            if (!ModelState.IsValid) return View(gymClass);

            _classService.ScheduleClass(gymClass);
            TempData["Success"] = $"{gymClass.Title} has been scheduled.";
            return RedirectToAction("Schedule");
        }

        // GET: Class/Cancel
        public ActionResult Cancel()
        {
            var activeClasses = _classService.GetActiveClasses();
            return View(activeClasses);
        }

        // POST: Class/Cancel
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cancel(int classId)
        {
            _classService.CancelClass(classId);
            TempData["Success"] = "Class has been cancelled.";
            return RedirectToAction("Cancel");
        }
        



    }

}