﻿using Blazor.FlexGrid.DataSet;
using Blazor.FlexGrid.DataSet.Http.Dto;
using Blazor.FlexGrid.Demo.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Blazor.FlexGrid.Demo.Server.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController : ControllerBase
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
        )
        {
            if (staticRepositoryCollections.Forecasts.TryGetValue(id, out var value))
            {
                return Ok(value);
            }

            return NotFound();
        }

        [HttpGet("[action]")]
        public IActionResult WeatherForecasts([FromQuery] FlexGridQueryDto queryParams)
        {
            var items = staticRepositoryCollections.Forecasts.Values.AsQueryable();

            if (!string.IsNullOrEmpty(queryParams.GroupExpression))
            {
                var groupedItems = items.GroupBy(queryParams.GroupExpression, "it")
                 .Select<GroupItem<WeatherForecast>>(ParsingConfig.Default, "new (it.Key as Key, it as Items)");

                return Ok(new
                {
                    Items = groupedItems.ToDictionary(g => g.Key, g => g.Items),
                    TotalCount = groupedItems.Count()
                });
            }

            var sortExp = queryParams?.SortExpression;
            if (!string.IsNullOrEmpty(sortExp))
            {
                if (queryParams.SortDescending)
                {
                    sortExp += " descending";
                }
                items = items.OrderBy(sortExp);
            }

            var result = items.Skip(queryParams.PageSize * queryParams.PageNumber).Take(queryParams.PageSize).ToList();

            return Ok(new
            {
                Items = result,
                TotalCount = staticRepositoryCollections.Forecasts.Count
            });
        }

        [HttpPost("[action]")]
        public IActionResult WeatherForecasts(FlexGridQueryDto queryParams, [FromBody] IEnumerable<FilterDefinition> filters)
        {
            var items = staticRepositoryCollections.Forecasts.Values.AsQueryable();

            var sortExp = queryParams.SortExpression;
            if (!string.IsNullOrEmpty(sortExp))
            {
                if (queryParams.SortDescending)
                {
                    sortExp += " descending";
                }
                items = items.OrderBy(sortExp);
            }

            return Ok(new
            {
                Items = items.Skip(queryParams.PageSize * queryParams.PageNumber).Take(queryParams.PageSize).ToList(),
                TotalCount = staticRepositoryCollections.Forecasts.Count
            });
        }


        [HttpGet("[action]")]
        public IEnumerable<WeatherForecast> WeatherForecastsSimple()
        {
            return staticRepositoryCollections.Forecasts.Values.Take(20);
        }

        [HttpGet("[action]")]
        public IEnumerable<ExpandoObject> WeatherForecastsSimpleExpando()
        {
            return staticRepositoryCollections.Forecasts.Values
                .Take(20)
                .Select(w =>
                {
                    dynamic forecast = new ExpandoObject();
                    forecast.Date = w.Date;
                    forecast.Summary = w.Summary;
                    forecast.TemperatureC = w.TemperatureC;
                    forecast.TemperatureF = w.TemperatureF;

                    return forecast as ExpandoObject;
                });
        }

        [HttpPost(nameof(WeatherForecast))]
        public IActionResult CreateWeatherForecast([FromBody] WeatherForecastCreateModel model)
        {
            var id = staticRepositoryCollections.Forecasts.Keys.Max() + 1;

            var weatherForecast = new WeatherForecast
            {
                Id = id,
                Date = model.Date.Value,
                Summary = model.Summary,
                TemperatureC = model.TemperatureC
            };

            if (staticRepositoryCollections.Forecasts.TryAdd(id, weatherForecast))
            {
                return Created(GetWeatherForecastUri(id), weatherForecast);
            }

            return Conflict();
        }

        [HttpPut("[action]")]
        public IActionResult UpdateWeatherForecast([FromBody] WeatherForecast weatherForecast)
        {
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
        public IActionResult Delete([FromQuery(Name = nameof(WeatherForecast.Id))] int id)
        {
            if (!staticRepositoryCollections.Forecasts.TryRemove(id, out var value))
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}

