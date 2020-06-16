using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace JSONDataValidationDemo1.Models
{
    public class Student
    {
        [Required(ErrorMessage = "Id is required")]
        [Range(1, 1024)]
        public int ID { get; set; }

        [Required]
        public string LastName { get; set; }
        [RegularExpression(pattern: @"^[A-Z][a-zA-Z]*$", ErrorMessage = "Invalid last name validated with custom regex")]
        public string FirstMidName { get; set; }

        [Required]
        [Annotations.Email(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        
        [Range(typeof(DateTime), "1/2/2004", "3/5/2004", ErrorMessage = "Value for {0} must be between {1} and {2}")]
        [Annotations.DisabledDates(new string[] { "2/2/2004", "3/3/2004", "4/4/2004" })]
        public DateTime FirstEnrollmentDate { get; set; }

        [Range(typeof(DateTime), "1/2/2004", "3/5/2004", ErrorMessage = "Value for {0} must be between {1} and {2}")]
        public DateTime SecondEnrollmentDate { get; set; }

        [MaxLength(10, ErrorMessage = "Comment too long")]
        public string Comment { get; set; }


    }
}