using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace JSONDataValidationDemo1.Annotations
{

    public class DisabledDatesAttribute : ValidationAttribute, IClientValidatable
    {
        public DisabledDatesAttribute(string[] disabledDates) : base($"The input date cannot be one of these dates {String.Join(", ", disabledDates)}")
        {
            DisabledDates = disabledDates;
        }

        public string[] DisabledDates { get; }


        public string GetParseErrorMessage() =>
           $"Unable to parse one of the disabled dates date";

        protected override ValidationResult IsValid(object value,
            ValidationContext validationContext)
        {

            DateTime inputDate = (DateTime)validationContext.ObjectInstance;
            foreach (string dateString in DisabledDates)
            {
                DateTime dateValue;
                if (DateTime.TryParse(dateString, out dateValue))
                {
                    if (inputDate == dateValue)
                    {
                        var errorMessage = FormatErrorMessage(validationContext.DisplayName);
                        return new ValidationResult(errorMessage);
                    }
                }
                else
                    return new ValidationResult(GetParseErrorMessage());

            }

            return ValidationResult.Success;
        }
   
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(
       ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule();
            rule.ErrorMessage = FormatErrorMessage(metadata.GetDisplayName());
            rule.ValidationParameters.Add("array", String.Join(", ", DisabledDates));
            rule.ValidationType = "disabled-dates";
            yield return rule;
        }
    }
}

