namespace UnitConversionApi.Services;

public class UnitConversionService : IUnitConversionService
{
    private static readonly Dictionary<string, double> LengthToMeter = new(StringComparer.OrdinalIgnoreCase)
    {
        ["m"] = 1,
        ["km"] = 1000,
        ["cm"] = 0.01,
        ["mm"] = 0.001,
        ["mi"] = 1609.344,
        ["yd"] = 0.9144,
        ["ft"] = 0.3048,
        ["in"] = 0.0254
    };

    private static readonly Dictionary<string, double> WeightToKilogram = new(StringComparer.OrdinalIgnoreCase)
    {
        ["kg"] = 1,
        ["g"] = 0.001,
        ["mg"] = 0.000001,
        ["lb"] = 0.45359237,
        ["oz"] = 0.028349523125
    };

    public bool TryConvert(
        double value,
        string fromUnit,
        string toUnit,
        out double result,
        out string? error)
    {
        result = 0;
        error = null;

        fromUnit = fromUnit.Trim();
        toUnit = toUnit.Trim();

        if (string.IsNullOrWhiteSpace(fromUnit) || string.IsNullOrWhiteSpace(toUnit))
        {
            error = "FromUnit and ToUnit are required.";
            return false;
        }

        if (LengthToMeter.TryGetValue(fromUnit, out var fromLength) &&
            LengthToMeter.TryGetValue(toUnit, out var toLength))
        {
            result = value * fromLength / toLength;
            return true;
        }

        if (WeightToKilogram.TryGetValue(fromUnit, out var fromWeight) &&
            WeightToKilogram.TryGetValue(toUnit, out var toWeight))
        {
            result = value * fromWeight / toWeight;
            return true;
        }

        if (IsTemperatureUnit(fromUnit) && IsTemperatureUnit(toUnit))
        {
            result = ConvertTemperature(value, fromUnit, toUnit);
            return true;
        }

        error = $"Unsupported conversion from '{fromUnit}' to '{toUnit}'. " +
                "Supported length units: m, km, cm, mm, mi, yd, ft, in. " +
                "Supported weight units: kg, g, mg, lb, oz. " +
                "Supported temperature units: c, f, k.";
        return false;
    }

    private static bool IsTemperatureUnit(string unit) =>
        unit.Equals("c", StringComparison.OrdinalIgnoreCase) ||
        unit.Equals("f", StringComparison.OrdinalIgnoreCase) ||
        unit.Equals("k", StringComparison.OrdinalIgnoreCase);

    private static double ConvertTemperature(double value, string from, string to)
    {
        double celsius = from.ToLowerInvariant() switch
        {
            "c" => value,
            "f" => (value - 32) * 5 / 9,
            "k" => value - 273.15,
            _ => throw new ArgumentException("Unsupported temperature unit.")
        };

        return to.ToLowerInvariant() switch
        {
            "c" => celsius,
            "f" => (celsius * 9 / 5) + 32,
            "k" => celsius + 273.15,
            _ => throw new ArgumentException("Unsupported temperature unit.")
        };
    }
}
