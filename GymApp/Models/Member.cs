using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

//Using data annotations to replace the if statements that were used for vcalidation in the original version

namespace GymApp.Models
{
    public class Member
    {
        public int MemID { get; set; }
        [Required(ErrorMessage = "Please enter a first name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First name must contain only letters")]
        [StringLength(30)]
        public string Fname { get; set; }
        [Required(ErrorMessage = "Please enter a last name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First name must contain only letters")]
        [StringLength(30)]
        public string Sname { get; set; }
        [Required(ErrorMessage = "Please enter an email")]
        [RegularExpression(@"^.+@(gmail|yahoo)\.(com|ie)$", ErrorMessage = "Must be a @gmail or @yahoo address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter a phone number")]
        [RegularExpression(@"^08\d{8}$", ErrorMessage = "Phone number must be 10 digits and start with 08")]
        public string Phone { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }
        public string Status { get; set; }
        public int Wallet { get; set; }
        public DateTime DateRegistered { get; set; }

    }
}