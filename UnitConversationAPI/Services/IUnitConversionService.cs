namespace UnitConversionApi.Services;

public interface IUnitConversionService
{
    bool TryConvert(double value, string fromUnit, string toUnit, out double result, out string? error);
}
