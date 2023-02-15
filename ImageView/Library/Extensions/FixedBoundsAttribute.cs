using System.ComponentModel.DataAnnotations;

namespace ImageViewer.Library.Extensions;

[AttributeUsage(AttributeTargets.Property)]
public class FixedBoundsAttribute : ValidationAttribute
{
    public FixedBoundsAttribute(int minValue, int maxValue, int initialValue, string errorMessage) : base(errorMessage)
    {
        MinValue = minValue;
        MaxValue = maxValue;
        InitialValue = initialValue;
    }

    public FixedBoundsAttribute(string errorMessage) : base(errorMessage)
    {
    }

    public int MinValue { get; set; }

    public int MaxValue { get; set; }

    public int InitialValue { get; set; }

    public string DisplayName { get; set; }

    public override bool IsValid(object value)
    {
        if (value == null)
        {
            return false;
        }
        // Assumes int value

        int propertyVal = Convert.ToInt32(value);

        if (propertyVal < MinValue || propertyVal > MaxValue)
        {
            return false;
        }

        return true;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        string propName = validationContext.MemberName;
        Log.Debug("Validating Settings Property: {name}", propName);

        bool validStatus = IsValid(value);

        if (validStatus)
        {
            return ValidationResult.Success;
        }

        Log.Warning("Validation failed for settings attrribute: " + validationContext.DisplayName);
        return new ValidationResult(ErrorMessageString);
    }
}