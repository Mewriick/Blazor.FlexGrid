using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Blazor.FlexGrid.Demo.Shared
{
    public class WeatherForecast : IValidatableObject
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        //[Required]
        [MinLength(4)]
        public string Summary { get; set; } = string.Empty;

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public override string ToString()
        {
            return $"Temp: {TemperatureC}, {TemperatureF}. Summary: {Summary}";
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield return new ValidationResult("Summary error", new List<string> { "Summary" });
        }
    }
}
