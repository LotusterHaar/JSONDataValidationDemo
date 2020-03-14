using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace JSONDataValidationDemo1.Annotations
{
    public class EmailAddressAttribute : RegularExpressionAttribute, IClientValidatable
    {

        public EmailAddressAttribute() : base(@"^(([^<>()[\]\\.,;:\s@""]+(\.[^<>()[\]\\.,;:\s@""]+)*)|("".+""))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))")
        {
            System.Diagnostics.Debug.WriteLine(this.Pattern);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRegexRule(this.ErrorMessage, this.Pattern)
            {
                ValidationType = "EmailAddress"
            };
        }

    }
}