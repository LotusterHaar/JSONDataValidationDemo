using JSONDataValidationDemo1.AppCode.Helpers;
using JSONDataValidationDemo1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JSONDataValidationDemo1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var InvalidObject = new Student() { Email = "test" };
            System.Diagnostics.Debug.WriteLine(InvalidObject.ValidateModel<Student>());

            InvalidObject = new Student() { ID = -7, Email = "someone@somewhere.com" };
            System.Diagnostics.Debug.WriteLine(InvalidObject.ValidateModel<Student>());

            InvalidObject = new Student() { ID = 117, Email = "someone@somewhere.com", Comment = string.Concat(Enumerable.Repeat("*", 500)) };
            System.Diagnostics.Debug.WriteLine(InvalidObject.ValidateModel<Student>());

            InvalidObject = new Student();
            System.Diagnostics.Debug.WriteLine(InvalidObject.ValidateModel<Student>());

            var validObject = new Student() { ID = 117, Email = "someone@somewhere.com", Comment = "This is test" };
            System.Diagnostics.Debug.WriteLine(validObject.ValidateModel<Student>());
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}