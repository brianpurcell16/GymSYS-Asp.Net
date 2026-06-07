using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GymApp.Services;
using GymApp.Models;

namespace GymApp.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;

        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        // GET: Member/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: Member/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Member member)
        {
            if (!ModelState.IsValid) return View(member);
            _memberService.RegisterMember(member);
            TempData["Success"] = "Thank you for registering!";

            return RedirectToAction("Register");
        }

        // GET: Member/Update
        public ActionResult Update()
        {
            return View();
        }


        // POST: Member/Search
        [HttpPost]
        public ActionResult Search(string fname, string sname)
        {
            var member = _memberService.GetMemberByName(fname, sname);
            if (member == null)
            {
                ModelState.AddModelError("", "Member not found. Please check the name and try again.");
                return View("Update");
            }
            return View("Update", member);
        }

        // POST: Member/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Member member, string originalFname, string originalSname)
        {
            if (!ModelState.IsValid) return View(member);
            _memberService.UpdateMember(member, originalFname, originalSname);
            TempData["Success"] = "Your details updated successfully!";
            return RedirectToAction("Update");
        }

        // GET: Member/Renew
        public ActionResult Renew()
        {
            return View(_memberService.GetInactiveMembers());
        }

        // POST: Member/Renew
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Renew(int memId)
        {
            _memberService.RenewMember(memId);
            TempData["Success"] = "Membership renewed successfully!";
            return RedirectToAction("Renew");
        }

        // GET: Member/Close
        public ActionResult Close()
        {
            return View(_memberService.GetActiveMembers());
        }

        // POST: Member/Close
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Close(int memId)
        {
            _memberService.CloseMembership(memId);
            TempData["Success"] = "Membership closed successfully!";
            return RedirectToAction("Close");
        }

        // GET: Member/AddFunds
        public ActionResult AddFunds()
        {
            return View();
        }

        // POST: Member/AddFunds
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddFunds(string fname, string sname, double amount)
        {
            if (string.IsNullOrEmpty(fname) || string.IsNullOrEmpty(sname))
            {
                ModelState.AddModelError("", "Please enter a first name and last name");
                return View();
            }

            if (amount <= 0)
            {
                ModelState.AddModelError("", "Please enter a valid amount greater than zero");
                return View();
            }

            var member = _memberService.GetMemberByName(fname, sname);
            if (member == null)
            {
                ModelState.AddModelError("", "Member not found. Please check the name and try again.");
                return View();
            }

            _memberService.AddFunds(fname, sname, amount);
            TempData["Success"] = $"€{amount} added to {fname} {sname}'s wallet.";
            return RedirectToAction("AddFunds");
        }


    }

}