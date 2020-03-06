using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace JSONDataValidationDemo1.AppCode.Helpers
{

    public static class ValidationExtensions
    {
        public static bool ValidateModel<T>(this string json) where T : new()
        {
            T model = new JavaScriptSerializer().Deserialize<T>(json);
            return ValidateModel<T>(model);
        }

        public static bool ValidateModel<T>(this T model) where T : new()
        {
            var validationContext = new ValidationContext(model, null, null);
            return Validator.TryValidateObject(model, validationContext, null, true);
        }
    }
}