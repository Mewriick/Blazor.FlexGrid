using Blazor.FlexGrid.Demo.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Blazor.FlexGrid.Demo.Server.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {
        private static string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet("[action]")]
        public IActionResult WeatherForecasts(int pageNumber, int pageSize, SortingParams sortingParams)
        {
            var rng = new Random();

            var items = Enumerable.Range(1, 100).Skip(pageSize * pageNumber).Take(pageSize).Select(index =>
                        new WeatherForecast
                        {
                            Date = DateTime.Now.AddDays(index),
                            TemperatureC = rng.Next(-20, 55),
                            Summary = Summaries[rng.Next(Summaries.Length)]

                        });

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
            var rng = new Random();

            return Enumerable.Range(1, 20).Select(index =>
                new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]

                });
        }
    }
}

