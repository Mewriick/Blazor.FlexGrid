using Blazor.FlexGrid.Demo.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Blazor.FlexGrid.Demo.Server.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {

        private readonly StaticRepositoryCollections staticRepositoryCollections;

        public SampleDataController(StaticRepositoryCollections staticRepositoryCollections)
        {
            this.staticRepositoryCollections = staticRepositoryCollections;
        }

        [HttpGet("[action]")]
        public IActionResult WeatherForecasts(
            [FromQuery] int pageNumber,
            [FromQuery] int pageSize,
            [FromQuery] SortingParams sortingParams
        ) {
            var items = staticRepositoryCollections.Forecasts.Values.AsQueryable();

            var sortExp = sortingParams?.SortExpression;
            if (!string.IsNullOrEmpty(sortExp))
            {
                if (sortingParams.SortDescending)
                {
                    sortExp += " descending";
                }
                items = items.OrderBy(sortExp);
            }

            return Ok(new {
                Items = items.Skip(pageSize * pageNumber).Take(pageSize),
                TotalCount = staticRepositoryCollections.Forecasts.Count
            });
        }

        [HttpGet("[action]")]
        public IEnumerable<WeatherForecast> WeatherForecastsSimple()
        {
            return staticRepositoryCollections.Forecasts.Values.Take(20);
        }

        [HttpPut("[action]")]
        public WeatherForecast UpdateWeatherForecast([FromBody] WeatherForecast weatherForecast)
        {
            weatherForecast.TemperatureC = weatherForecast.TemperatureC + 1;

            return weatherForecast;
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromQuery]int id)
        {
            return NoContent();
        }
    }
}

