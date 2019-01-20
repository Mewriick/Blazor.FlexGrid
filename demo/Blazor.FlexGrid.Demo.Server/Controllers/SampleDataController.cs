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
        public IActionResult WeatherForecasts(int pageNumber, int pageSize, SortingParams sortingParams)
        {
            var items = staticRepositoryCollections.Forecasts
                .Skip(pageSize * pageNumber)
                .Take(pageSize);

            items = string.IsNullOrEmpty(sortingParams.SortExpression)
                 ? items
                 : items.AsQueryable().OrderBy(sortingParams.SortExpression).ToList();

            return Ok(
                new
                {
                    Items = items,
                    TotalCount = 100
                });
        }

        [HttpGet("[action]")]
        public IEnumerable<WeatherForecast> WeatherForecastsSimple()
        {
            return staticRepositoryCollections.Forecasts.Take(20);
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

