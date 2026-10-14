# Unit Conversion REST API

A simple ASP.NET Core RESTful API built with **.NET 10** for converting numeric values between compatible units.

## Features

- RESTful `POST` endpoint
- Length conversion
- Weight conversion
- Temperature conversion
- JSON request/response
- Clear validation and error responses
- Dependency injection with a dedicated conversion service
- No database or external dependencies required

## Supported Units

### Length

`m`, `meter`, `meters`, `km`, `kilometer`, `kilometers`, `cm`, `centimeter`, `centimeters`, `mm`, `millimeter`, `millimeters`, `mi`, `mile`, `miles`, `yd`, `yard`, `yards`, `ft`, `foot`, `feet`, `in`, `inch`, `inches`

### Weight

`g`, `gram`, `grams`, `kg`, `kilogram`, `kilograms`, `mg`, `milligram`, `milligrams`, `lb`, `lbs`, `pound`, `pounds`, `oz`, `ounce`, `ounces`

### Temperature

`c`, `celsius`, `f`, `fahrenheit`, `k`, `kelvin`

Only units belonging to the same category can be converted.

## Prerequisites

Install the **.NET 10 SDK**.

Verify the installation:

```bash
dotnet --version
```

The project targets:

```text
net10.0
```

## Run Locally

1. Extract the ZIP file.
2. Open a terminal in the project directory.
3. Restore dependencies:

```bash
dotnet restore
```

4. Build the project:

```bash
dotnet build
```

5. Run the API:

```bash
dotnet run
```

The terminal will display the local URLs, for example:

```text
http://localhost:5000
https://localhost:7000
```

Use the URL shown by your terminal.

## API Endpoint

### Convert a value

**POST**

```text
/api/conversion
```

### Request

```json
{
  "value": 10,
  "from": "km",
  "to": "m"
}
```

### Response

```json
{
  "value": 10,
  "from": "km",
  "to": "m",
  "result": 10000
}
```

## Example Requests

### Kilometers to miles

```json
{
  "value": 10,
  "from": "km",
  "to": "mi"
}
```

### Kilograms to pounds

```json
{
  "value": 5,
  "from": "kg",
  "to": "lb"
}
```

### Celsius to Fahrenheit

```json
{
  "value": 100,
  "from": "celsius",
  "to": "fahrenheit"
}
```

## Testing with cURL

```bash
curl -X POST "http://localhost:5000/api/conversion" ^
  -H "Content-Type: application/json" ^
  -d "{"value":10,"from":"km","to":"m"}"
```

On PowerShell, you can use:

```powershell
Invoke-RestMethod `
  -Uri "http://localhost:5000/api/conversion" `
  -Method Post `
  -ContentType "application/json" `
  -Body '{"value":10,"from":"km","to":"m"}'
```

## Error Example

Trying to convert a length unit to a weight unit:

```json
{
  "value": 10,
  "from": "km",
  "to": "kg"
}
```

Returns HTTP `400 Bad Request`:

```json
{
  "error": "Cannot convert between different unit types: km (Length) and kg (Weight)."
}
```

## Project Structure

```text
UnitConversionApi/
├── Controllers/
│   └── ConversionController.cs
├── Models/
│   ├── ConversionRequest.cs
│   └── ConversionResponse.cs
├── Services/
│   ├── IUnitConversionService.cs
│   └── UnitConversionService.cs
├── Program.cs
├── UnitConversionApi.csproj
└── README.md
```

## Architecture

The API follows a simple separation of responsibilities:

- **Controller**: Handles HTTP requests and responses.
- **Models**: Defines request and response contracts.
- **Service**: Contains unit-conversion business logic.
- **Program.cs**: Configures dependency injection and the ASP.NET Core pipeline.

## Notes

- Conversion is performed in memory.
- No database is required.
- No third-party NuGet packages are required.
- Unit names are case-insensitive.
- Values can be integers or decimal numbers.

## License

MIT
