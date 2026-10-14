using Microsoft.AspNetCore.Mvc;
using UnitConversionApi.Models;
using UnitConversionApi.Services;

namespace UnitConversionApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConversionController : ControllerBase
{
    private readonly IUnitConversionService _conversionService;

    public ConversionController(IUnitConversionService conversionService)
    {
        _conversionService = conversionService;
    }

    /// <summary>
    /// Converts a numeric value from one supported unit to another.
    /// </summary>
    [HttpPost]
    public IActionResult Convert([FromBody] UnitConversionRequest request)
    {
        if (double.IsNaN(request.Value) || double.IsInfinity(request.Value))
        {
            return BadRequest(new { message = "Value must be a valid finite number." });
        }

        if (!_conversionService.TryConvert(
                request.Value,
                request.FromUnit,
                request.ToUnit,
                out var convertedValue,
                out var error))
        {
            return BadRequest(new { message = error });
        }

        var response = new UnitConversionResponse
        {
            InputValue = request.Value,
            FromUnit = request.FromUnit.Trim().ToLowerInvariant(),
            ConvertedValue = Math.Round(convertedValue, 6),
            ToUnit = request.ToUnit.Trim().ToLowerInvariant()
        };

        return Ok(response);
    }

    /// <summary>
    /// Returns the units supported by the API.
    /// </summary>
    [HttpGet("units")]
    public IActionResult GetSupportedUnits()
    {
        return Ok(new
        {
            length = new[] { "m", "km", "cm", "mm", "mi", "yd", "ft", "in" },
            weight = new[] { "kg", "g", "mg", "lb", "oz" },
            temperature = new[] { "c", "f", "k" }
        });
    }
}
