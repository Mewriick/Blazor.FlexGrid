﻿using Blazor.FlexGrid.Demo.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

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
        public IActionResult WeatherForecasts()
        {
            var rng = new Random();
            return Ok(
                new
                {
                    Items = Enumerable.Range(1, 20).Select(index =>
                        new WeatherForecast
                        {
                            Date = DateTime.Now.AddDays(index),
                            TemperatureC = rng.Next(-20, 55),
                            Summary = Summaries[rng.Next(Summaries.Length)]

                        }),
                    TotalCount = 100
                });
        }
    }
}

