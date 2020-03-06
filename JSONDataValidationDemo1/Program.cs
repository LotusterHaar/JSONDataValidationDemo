using JSONDataValidationDemo1.AppCode.Helpers;
using JSONDataValidationDemo1.Models;
using System;
using System.Linq;

namespace JSONDataValidationDemo1
{
    class Prgogram
    {
        static void Main(string[] args)
        {
            var InvalidObject = new Student() { Email = "test" };
            Console.WriteLine(InvalidObject.ValidateModel<Student>());

            InvalidObject = new Student() { ID = -7, Email = "someone@somewhere.com" };
            Console.WriteLine(InvalidObject.ValidateModel<Student>());

            InvalidObject = new Student() { ID = 117, Email = "someone@somewhere.com", Comment = string.Concat(Enumerable.Repeat("*", 500)) };
            Console.WriteLine(InvalidObject.ValidateModel<Student>());

            InvalidObject = new Student();
            Console.WriteLine(InvalidObject.ValidateModel<Student>());

            var validObject = new Student() { ID = 117, Email = "someone@somewhere.com", Comment = "This is test" };
            Console.WriteLine(validObject.ValidateModel<Student>());

            Console.ReadLine();
        }
    }
}