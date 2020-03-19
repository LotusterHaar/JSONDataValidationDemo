using JSONDataValidationDemo1.Annotations;
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
        //public List<Dictionary<string, string>> ValidationRules
        //{

        //    get; set;

        //    //get
        //    //{
        //    //    Dictionary<string, string> dict = new Dictionary<string, string>() { { "a.1", "Dog" }, { "a.2", "Cat" }, { "a.3", "Pig" } };
        //    //    List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
        //    //    list.Add(dict);
        //    //    return list;
        //    //}
        //    //set
        //    //{
        //    //    ValidationRules = value;
        //    //}

        //}

    }
}