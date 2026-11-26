using System.Globalization;

namespace ParcelTrack.Services;

public sealed class DeliveryFeeCalculator : IDeliveryFeeCalculator
{
    private const decimal BaseFee = 500m;
    private const decimal PerKgFee = 150m;
    private const decimal OutOfMetroSurcharge = 300m;

    public decimal Calculate(decimal weightKg, string? parish)
    {
        var safeWeight = weightKg < 0m ? 0m : weightKg;
        var roundedKg = (decimal)Math.Ceiling((double)safeWeight);
        var fee = BaseFee + (PerKgFee * roundedKg);

        if (!IsKingstonOrStAndrew(parish))
        {
            fee += OutOfMetroSurcharge;
        }

        return fee;
    }

    private static bool IsKingstonOrStAndrew(string? parish)
    {
        if (string.IsNullOrWhiteSpace(parish))
        {
            return false;
        }

        var normalized = parish
            .Trim()
            .ToLowerInvariant()
            .Replace(".", string.Empty)
            .Replace("-", " ");

        while (normalized.Contains("  ", StringComparison.Ordinal))
        {
            normalized = normalized.Replace("  ", " ", StringComparison.Ordinal);
        }

        return normalized is "kingston" or "st andrew" or "saint andrew" or "kingston and st andrew" or "kingston & st andrew";
    }
}