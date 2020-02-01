using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace ImageViewer.Library.Extensions
{
    public static class DataAnotation
    {
        public static string ToDescErrorsString(this IEnumerable<ValidationResult> source, string messageEmptyCollection = null)
        {
            StringBuilder result = new StringBuilder();

            IEnumerable<ValidationResult> validationResults = source as ValidationResult[] ?? source.ToArray();
            if (validationResults.Any())
            {
                result.AppendLine("We found the next validations errors:");
                validationResults.ToList().ForEach(s => result.AppendFormat("  {0} --> {1}{2}", s.MemberNames.FirstOrDefault(), s.ErrorMessage, Environment.NewLine));
            }
            else
                result.AppendLine(messageEmptyCollection ?? string.Empty);

            return result.ToString();
        }


        public static IEnumerable<ValidationResult> ValidateObject(this object source)
        {
            ValidationContext valContext = new ValidationContext(source, null, null);
            var result = new List<ValidationResult>();
            Validator.TryValidateObject(source, valContext, result, true);

            return result;
        }

        public static bool IsValid(this SettingsProperty source)
        {
            source.
            if (instance.ExcludeRange)
            {
                return value < instance.MinValue && value > instance.MaxValue;
            }

            return value >= instance.MinValue && value <= instance.MaxValue;
        }
    }
}