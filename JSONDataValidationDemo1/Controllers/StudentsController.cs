using JSONDataValidationDemo1.DAL;
using JSONDataValidationDemo1.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace JSONDataValidationDemo1.Controllers
{
    public class StudentsController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: Students
        public ActionResult Index()
        {
            return View(db.Students.ToList());
        }

        public JsonResult StudentsJson()
        {
            var obj = new Student { ID = 0, FirstMidName = "Carson", Email = "lotus", LastName = "Alexander", EnrollmentDate = DateTime.Parse("2005-09-01") };

            Type type = obj.GetType();

            IEnumerable<string> propertyNames = type.GetProperties().Select(p => p.Name);

            List<Dictionary<string, string>> validationRules = new List<Dictionary<string, string>>();
            foreach (string property in propertyNames)
            {

                PropertyInfo propertyInfo = type.GetProperty(property);

                System.Diagnostics.Debug.WriteLine($"Attributes: {property}");
                var metadata = ModelMetadataProviders.Current.GetMetadataForProperty(null, typeof(Student), property);
                var rules = metadata.GetValidators(ControllerContext).SelectMany(v => v.GetClientValidationRules());

                Dictionary<string, string> validationAttributes = new Dictionary<string, string>();

                foreach (ModelClientValidationRule rule in rules)
                {
                    string key = "data-val-" + rule.ValidationType;
                    validationAttributes.Add(key, HttpUtility.HtmlEncode(rule.ErrorMessage ?? string.Empty));
                    key = key + "-";
                    foreach (KeyValuePair<string, object> pair in rule.ValidationParameters)
                    {
                        validationAttributes.Add(key + pair.Key,
                            HttpUtility.HtmlAttributeEncode(
                                pair.Value != null ? Convert.ToString(pair.Value, CultureInfo.InvariantCulture) : string.Empty));
                    }

                }
                validationRules.Add(validationAttributes);

                System.Diagnostics.Debug.WriteLine($"name: {property}\n validationRules:{string.Join(Environment.NewLine, validationAttributes)}");

            }

            obj.ValidationRules = validationRules;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }


        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,LastName,FirstMidName,Email,EnrollmentDate,Comment")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return Json(student);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,LastName,FirstMidName,Email,EnrollmentDate,Comment")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
