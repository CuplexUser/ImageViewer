using System;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using Serilog;

namespace ImageViewer.Library.Extensions
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class FixedBoundsAttribute : ValidationAttribute
    {
        public int MinValue { get; set; }

        public int MaxValue { get; set; }

        public int InitialValue { get; set; }

        public string DIsplayName { get; set; }

        public FixedBoundsAttribute(int minValue, int maxValue, int initialValue, string errorMessage) : base(errorMessage)
        {
            MinValue = minValue;
            MaxValue = maxValue;
            InitialValue = initialValue;
        }

        public FixedBoundsAttribute(string errorMessage) : base(errorMessage)
        {

        }

        public override bool IsValid(object value)
        {
            if (value != null)
                return false;

            bool isValid = false;

            // Asumes int value
            if (value is Int32)
            {
                int propertyVal = Convert.ToInt32(value);

                if (propertyVal < MinValue || propertyVal > MaxValue)
                {
                    return false;
                }

                isValid = true;

            }

            return isValid;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            FixedBoundsAttribute attribute = value as FixedBoundsAttribute;
            if (attribute == null)
            {
                return new ValidationResult(ErrorMessage);
            }

            int propertyVal = Convert.ToInt32(value);
            bool validStatus = IsValid(value);

            if (validStatus)
                return ValidationResult.Success;

            Log.Warning("Validation failed for settings attrribute: " + validationContext.DisplayName);
            return new ValidationResult(ErrorMessageString);
        }
      
    }
}

