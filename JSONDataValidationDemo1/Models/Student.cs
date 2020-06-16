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

        [Range(typeof(DateTime), "1/2/2004", "3/4/2004", ErrorMessage = "Value for {0} must be between {1} and {2}")]
        public DateTime EnrollmentDate { get; set; }

        [MaxLength(10, ErrorMessage = "Comment too long")]
        public string Comment { get; set; }


    }
}