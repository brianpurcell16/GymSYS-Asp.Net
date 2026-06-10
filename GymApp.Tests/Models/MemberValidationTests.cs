using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymApp.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.DataAnnotations;

namespace GymApp.Tests.Models
{
    [TestClass]
    public class MemberValidationTests
    {
        public List<ValidationResult> ValidateModel(object model)
        {
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(model, new ValidationContext(model), results, true);
            return results;
        }

        [TestMethod]
        public void Member_WithValidData_PassesValidation()
        {
            var member = new Member
            {
                Fname = "Brian",
                Sname = "Purcell",
                Email = "brian@gmail.com",
                Phone = "0870537719",
                DOB = new DateTime(2000, 1, 1)
            };
            Assert.AreEqual(0, ValidateModel(member).Count);
        }

        [TestMethod]
        public void Member_WithEmptyFname_FailsValidation()
        {
            var member = new Member { Fname = "" };
            Assert.IsTrue(ValidateModel(member).Any(e => e.MemberNames.Contains("Fname")));
        }

        [TestMethod]
        public void Member_WithDigitInFname_FailsValidation()
        {
            var member = new Member { Fname = "Brian3" };
            Assert.IsTrue(ValidateModel(member).Any(e => e.MemberNames.Contains("Fname")));
        }

        [TestMethod]
        public void Member_WithPhoneNotStartingWith08_FailsValidation()
        {
            var member = new Member { Phone = "1234567890" };
            Assert.IsTrue(ValidateModel(member).Any(e => e.MemberNames.Contains("Phone")));
        }

        [TestMethod]
        public void Member_WithInvalidEmailDomain_FailsValidation()
        {
            var member = new Member { Email = "brian@hotmail.com" };
            Assert.IsTrue(ValidateModel(member).Any(e => e.MemberNames.Contains("Email")));
        }

    }
}
