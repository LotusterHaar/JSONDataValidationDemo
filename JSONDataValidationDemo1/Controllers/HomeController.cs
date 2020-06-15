using JSONDataValidationDemo1.Extensions;
using JSONDataValidationDemo1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace JSONDataValidationDemo1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult JsonValidationRules()
        {
            List<dynamic> form = new List<dynamic>();

            Student data = new Student { ID = 0, FirstMidName = "Carson", Email = "lotus", LastName = "Alexander", EnrollmentDate = DateTime.Parse("2005-09-01") };
            form = ConvertObjectToJsonWithValidationRules(data);

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

        /// <summary>
        /// http://localhost:51214/Home/JsonValidatedFields
        /// </summary>
        /// <returns>Json structure with validated fields and validation rules</returns>
        public ActionResult JsonValidatedFields()
        {
            List<dynamic> form = new List<dynamic>();
            Student student = new Student { ID = 0, FirstMidName = "L.A.", Email = "lotus", LastName = "Haar1", EnrollmentDate = DateTime.Parse("2005-09-01") };
            form = ConvertObjectToJsonWithValidatedFields(student);
           
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
                        validationAttributes.Add("rule", key);
                        validationAttributes.Add("message", HttpUtility.HtmlEncode(rule.ErrorMessage ?? string.Empty));
                        System.Diagnostics.Debug.WriteLine($"ValidationAttributes - {key}, {HttpUtility.HtmlEncode(rule.ErrorMessage ?? string.Empty)}");
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
                                    List<dynamic> additionalValidationAttributes = new List<dynamic>();
                                    Dictionary<string, dynamic> additionalValidationAttribute = new Dictionary<string, dynamic>();
                                    foreach (KeyValuePair<string, object> pair in rule.ValidationParameters)
                                    {
                                        additionalValidationAttribute.Add("rule", key + pair.Key);
                                        additionalValidationAttribute.Add("message", HttpUtility.HtmlAttributeEncode(
                                                 pair.Value != null ? Convert.ToString(pair.Value, CultureInfo.InvariantCulture) : string.Empty));
                                        additionalValidationAttributes.Add(additionalValidationAttribute);
                                        additionalValidationAttribute = new Dictionary<string, dynamic>();
                                    }
                                    validationAttributes.Add("additional-rules", additionalValidationAttributes);
                                    additionalValidationAttributes = new List<dynamic>();
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
                        if (!validationAttributes.ContainsKey("isValid"))
                        {
                            validationAttributes.Add("isValid", true);
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


    }
}