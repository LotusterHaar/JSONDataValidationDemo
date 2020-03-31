using JSONDataValidationDemo1.Annotations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        [RegularExpression (pattern: @"^[A-Z][a-zA-Z]*$", ErrorMessage ="Invalid last name validated with custom regex")]
        public string FirstMidName { get; set; }

        [Required]
        [Annotations.EmailAttribute(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        
        //Telephone number

        public DateTime EnrollmentDate { get; set; }

        //public virtual ICollection<Enrollment> Enrollments { get; set; }

        [MaxLength(255, ErrorMessage = "Comment too long")]
        public string Comment { get; set; }


    }
}