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
        )
        {
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
            [FromQuery] SortingParams sortingParams)
        {
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

            var result = items.Skip(pageSize * pageNumber).Take(pageSize).ToList();

            return Ok(new
            {
                Items = result,
                TotalCount = staticRepositoryCollections.Forecasts.Count
            });
        }

        [HttpPost("[action]")]
        public IActionResult WeatherForecasts(
            [FromQuery] int pageNumber,
            [FromQuery] int pageSize,
            [FromQuery] SortingParams sortingParams,
            [FromBody] IEnumerable<FilterDefinition> filters)
        {
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

            return Ok(new
            {
                Items = items.Skip(pageSize * pageNumber).Take(pageSize),
                TotalCount = staticRepositoryCollections.Forecasts.Count
            });
        }

        /*
        [HttpGet("[action]")]
        public IActionResult WeatherForecasts(
            [FromQuery] int pageNumber,
            [FromQuery] int pageSize,
            [FromQuery] SortingParams sortingParams,
            [FromQuery] string groupExpression)
        {
            var items = staticRepositoryCollections.Forecasts.Values.AsQueryable();

            if (string.IsNullOrEmpty(groupExpression))
                return WeatherForecasts(pageNumber, pageSize, sortingParams);
            else
            {
                var param = Expression.Parameter(typeof(WeatherForecast));
                var property = Expression.PropertyOrField(param, groupExpression);

                var keyPropertyConstructors = typeof(KeyProperty).GetConstructors();
                var newExpr = Expression.New(keyPropertyConstructors.FirstOrDefault(c => c.GetParameters()[0].ParameterType == property.Type)
                    , property);
                var lambda = Expression.Lambda<Func<WeatherForecast, KeyProperty>>(newExpr, param);
                var groupedItemsAfterGrouping = items.GroupBy(lambda)
                        .Select(grp => new GroupItem<WeatherForecast>(grp.Key.Key, grp.ToList()));

                var groupedItemsAfterPaging = groupedItemsAfterGrouping
                    .Skip(pageSize * pageNumber)
                    .Take(pageSize);

                var groupedItemsAfterSorting = new List<GroupItem<WeatherForecast>>();
                var sortExp = sortingParams?.SortExpression;
                if (!string.IsNullOrEmpty(sortExp))
                {
                    if (sortingParams.SortDescending)
                    {
                        sortExp += " descending";
                    }

                    foreach (var groupItem in groupedItemsAfterPaging)
                    {
                        groupedItemsAfterSorting.Add(new GroupItem<WeatherForecast>(groupItem.Key,
                            groupItem.Items.AsQueryable().OrderBy(sortExp)));
                    }
                }
                else
                    groupedItemsAfterSorting = groupedItemsAfterPaging.ToList();


                return Ok(new
                {
                    Items = groupedItemsAfterSorting.SelectMany(grp => grp.Items),
                    TotalCount = groupedItemsAfterGrouping.Count()
                });
            }
        }*/

        [HttpGet("[action]")]
        public IEnumerable<WeatherForecast> WeatherForecastsSimple()
        {
            return staticRepositoryCollections.Forecasts.Values.Take(20);
        }

        [HttpPost(nameof(WeatherForecast))]
        public IActionResult CreateWeatherForecast([FromBody] WeatherForecastCreateModel model)
        {
            var id = staticRepositoryCollections.Forecasts.Keys.Max() + 1;

            var weatherForecast = new WeatherForecast
            {
                Id = id,
                Date = model.Date,
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

