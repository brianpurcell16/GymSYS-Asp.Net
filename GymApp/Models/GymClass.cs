using GymApp.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GymApp.Models
{
    public class GymClass
    {
        public int ClassID { get; set; }
        [Required(ErrorMessage = "Please enter a title for the class")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Title must contain only letters and spaces")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Please enter a price for the class")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Price must be a positive integer")]
        [Range(0, 100, ErrorMessage = "Price must be less than 100 euro")]
        public int Price { get; set; }
        public string Status { get; set; }
        [Required(ErrorMessage = "Please enter a time for the class")]
        public string CTime { get; set; }
        [Required(ErrorMessage = "Please enter a date for the class")]
        [DataType(DataType.Date)]
        [RegularExpression(@"^\d{4}-\d{2}-\d{2}$", ErrorMessage = "Date must be in the format YYYY-MM-DD")]
        [FutureDate]
        public DateTime CDate { get; set; }
        [Required(ErrorMessage = "Please enter the capacity of the class")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Capacity must be a positive integer")]
        [Range(1, 100, ErrorMessage = "Capacity must be between 1 and 100")]
        public int Capacity { get; set; }
        public int AvailableSpaces { get; set; }
    }
}