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

        private static string GetWeatherForecastUri(int id)
            => $"/api/SampleData/{nameof(WeatherForecast)}?{nameof(WeatherForecast.Id)}={id}";

        [HttpGet(nameof(WeatherForecast))]
        public IActionResult GetWeatherForecast(
            [FromQuery(Name = nameof(WeatherForecast.Id))] int id
        ) {
            if (staticRepositoryCollections.Forecasts.TryGetValue(id, out var value))
            {
                return Ok(value);
            }

            return NotFound();
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
        public IActionResult UpdateWeatherForecast(
            [FromBody] WeatherForecast weatherForecast
        ) {
            var id = weatherForecast.Id;
            if (staticRepositoryCollections.Forecasts.TryGetValue(id, out var value))
            {
                if (staticRepositoryCollections.Forecasts.TryUpdate(id, weatherForecast, value))
                {
                    return NoContent();
                }
            }
            else if (staticRepositoryCollections.Forecasts.TryAdd(id, weatherForecast))
            {
                return Created(GetWeatherForecastUri(id), weatherForecast);
            }

            return Conflict();
        }

        [HttpDelete("[action]")]
        public IActionResult Delete(
            [FromQuery(Name = nameof(WeatherForecast.Id))] int id
        ) {
            if (!staticRepositoryCollections.Forecasts.TryRemove(id, out var value))
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}

