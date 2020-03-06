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
            var InvalidObject = new Student() { ID = -7, Email = "someone@somewhere.com" };
            Console.WriteLine(InvalidObject.ValidateModel<Student>());
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