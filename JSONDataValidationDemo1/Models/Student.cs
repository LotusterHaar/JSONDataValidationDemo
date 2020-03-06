using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JSONDataValidationDemo1.Models
{
    public class Student
    {
        [Required(ErrorMessage = "Id is required")]
        [Range(1, int.MaxValue)]
        public int ID { get; set; }
        public string LastName { get; set; }
        public string FirstMidName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        public DateTime EnrollmentDate { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; }

        [MaxLength(255, ErrorMessage = "Comment too long")]
        public string Comment { get; set; }
    }
}