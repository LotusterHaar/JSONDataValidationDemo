using JSONDataValidationDemo1.DAL;
using JSONDataValidationDemo1.Extensions;
using JSONDataValidationDemo1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

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

        public ActionResult StudentsJson()
        {
            List<dynamic> form = new List<dynamic>();

            Student data = new Student { ID = 0, FirstMidName = "Carson", Email = "lotus", LastName = "Alexander", EnrollmentDate = DateTime.Parse("2005-09-01") };
            form  = ConvertObjectToJsonWithValidationRules(data);

            return Json(form, JsonRequestBehavior.AllowGet);
        }

        public List<dynamic> ConvertObjectToJsonWithValidationRules<T>(T data)
        {
            Type type = data.GetType();

            List<dynamic> form = new List<dynamic>();
            Dictionary<string, dynamic> field = new Dictionary<string, dynamic>();
            foreach (PropertyInfo property in type.GetProperties())
            {
                string propertyName = property.Name;
                string propertyType = property.PropertyType.ToString().Replace("System.", "");
                if (property.PropertyType.IsNumericType())
                    propertyType = "Number";
                string propertyValue = string.Empty;
                if (property.GetValue(data) != null)
                    propertyValue = property.GetValue(data).ToString();
                field.Add("name", propertyName);
                field.Add("type", propertyType);
                field.Add("value", propertyValue);

                List<Dictionary<string, string>> validationRules = new List<Dictionary<string, string>>();

                PropertyInfo propertyInfo = type.GetProperty(propertyName);

                var metadata = ModelMetadataProviders.Current.GetMetadataForProperty(null, typeof(Student), propertyName);
                var rules = metadata.GetValidators(ControllerContext).SelectMany(v => v.GetClientValidationRules());

                Dictionary<string, string> validationAttributes = new Dictionary<string, string>();

                foreach (ModelClientValidationRule rule in rules)
                {
                    string key = rule.ValidationType;
                    validationAttributes.Add(key, HttpUtility.HtmlEncode(rule.ErrorMessage ?? string.Empty));
                    key = key + "-";
                    foreach (KeyValuePair<string, object> pair in rule.ValidationParameters)
                    {
                        validationAttributes.Add(key + pair.Key,
                            HttpUtility.HtmlAttributeEncode(
                                pair.Value != null ? Convert.ToString(pair.Value, CultureInfo.InvariantCulture) : string.Empty));
                    }
                    validationRules.Add(validationAttributes);
                    validationAttributes = new Dictionary<string, string>();
                }
                field.Add("validation", validationRules);
                form.Add(field);
                field = new Dictionary<string, dynamic>();
            }
            return form;
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
        public ActionResult Create([Bind(Include = "ID,LastName,FirstMidName,Email,EnrollmentDate,Comment")] Student data)
        {
            List<dynamic> form = new List<dynamic>();
            Student student = new Student { ID = 0, FirstMidName = "L.A.", Email = "lotus", LastName = "Haar1", EnrollmentDate = DateTime.Parse("2005-09-01") };
            form = ConvertObjectToJsonWithValidatedFields(student);
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return Json(form, JsonRequestBehavior.AllowGet);
        }

        public List<dynamic> ConvertObjectToJsonWithValidatedFields<T>(T data)
        {

            Type type = data.GetType();

            List<dynamic> form = new List<dynamic>();
            Dictionary<string, dynamic> field = new Dictionary<string, dynamic>();
            foreach (PropertyInfo property in type.GetProperties())
            {
                string propertyName = property.Name;
                string propertyType = property.PropertyType.ToString().Replace("System.", "");
                if (property.PropertyType.IsNumericType())
                    propertyType = "Number";
                string propertyValueString = "NULL";
                var propertyValue = property.GetValue(data);
                if (propertyValue != null)
                {
                    propertyValueString = property.GetValue(data).ToString();
                    field.Add("name", propertyName);
                    field.Add("type", propertyType);
                    field.Add("value", propertyValueString);

                    List<Dictionary<string, dynamic>> validationRules = new List<Dictionary<string, dynamic>>();
                    Dictionary<string, dynamic> validationAttributes = new Dictionary<string, dynamic>();

                    var metadata = ModelMetadataProviders.Current.GetMetadataForProperty(null, typeof(Student), propertyName);
                    var rules = metadata.GetValidators(ControllerContext).SelectMany(v => v.GetClientValidationRules());
                    foreach (ModelClientValidationRule rule in rules)
                    {

                        string key = rule.ValidationType;
                        validationAttributes.Add(key, HttpUtility.HtmlEncode(rule.ErrorMessage ?? string.Empty));
                        System.Diagnostics.Debug.WriteLine(key, HttpUtility.HtmlEncode(rule.ErrorMessage ?? string.Empty));
                        key = key + "-";
                        var context = new ValidationContext(propertyValue, null, null);
                        var results = new List<ValidationResult>();
                        var attributes = typeof(Student)
                                .GetProperty(propertyName)
                                .GetCustomAttributes(true)
                                .OfType<ValidationAttribute>()
                                .ToArray();
                        foreach (var attribute in attributes)
                        {
                            ValidationAttribute[] singleAttributeList = new ValidationAttribute[] { attribute };
                            if (!Validator.TryValidateValue(propertyValue, context, results, singleAttributeList))
                            {
                                foreach (var result in results)
                                {
                                    foreach (KeyValuePair<string, object> pair in rule.ValidationParameters)
                                    {
                                        validationAttributes.Add(key + pair.Key,
                                            HttpUtility.HtmlAttributeEncode(
                                                 pair.Value != null ? Convert.ToString(pair.Value, CultureInfo.InvariantCulture) : string.Empty));
                                    }

                                    if (attribute.FormatErrorMessage(propertyName).Equals(rule.ErrorMessage))
                                    {

                                        validationAttributes.Add("isValid", false);
                                    }
                                    else
                                    {
                                        validationAttributes.Add("isValid", true);
                                        System.Diagnostics.Debug.WriteLine($"{propertyName}: {key} =  TRUE ---- {attribute.FormatErrorMessage(propertyName)} != {HttpUtility.HtmlEncode(rule.ErrorMessage ?? string.Empty)}");
                                    }
                                }
                            }
                        }
                        validationRules.Add(validationAttributes);
                        validationAttributes = new Dictionary<string, dynamic>();
                    }
                    field.Add("validation", validationRules);
                    form.Add(field);
                    field = new Dictionary<string, dynamic>();
                }
                else
                {
                    List<Dictionary<string, dynamic>> validationRules = new List<Dictionary<string, dynamic>>();
                    Dictionary<string, dynamic> validationAttributes = new Dictionary<string, dynamic>();
                    validationRules.Add(validationAttributes);

                    field.Add("name", propertyName);
                    field.Add("type", propertyType);
                    field.Add("value", propertyValueString);
                    field.Add("validation", validationRules);
                    form.Add(field);
                    field = new Dictionary<string, dynamic>();
                }
            }
            return form;
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
