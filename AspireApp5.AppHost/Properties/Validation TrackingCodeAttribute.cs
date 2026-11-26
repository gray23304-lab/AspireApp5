using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ParcelTrack.Validation;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed partial class TrackingCodeAttribute : ValidationAttribute
{
    public TrackingCodeAttribute()
        : base("Tracking code must look like PT-123456")
    {
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null)
        {
            return ValidationResult.Success;
        }

        if (value is not string str || string.IsNullOrEmpty(str))
        {
            return ValidationResult.Success;
        }

        if (TrackingCodeRegex().IsMatch(str))
        {
            return ValidationResult.Success;
        }

        return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
    }

    [GeneratedRegex("^PT-\\d{6}$", RegexOptions.Compiled)]
    private static partial Regex TrackingCodeRegex();
}
