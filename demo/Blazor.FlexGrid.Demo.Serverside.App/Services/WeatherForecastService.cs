using Blazor.FlexGrid.DataSet;
using Blazor.FlexGrid.DataSet.Options;
using Blazor.FlexGrid.Demo.Shared;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Blazor.FlexGrid.Demo.Serverside.App.Services
{
    public class WeatherForecastService : ILazyDataSetLoader<WeatherForecast>
    {
        private static string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public Task<WeatherForecast[]> GetForecastAsync(DateTime startDate)
        {
            var rng = new Random();
            return Task.FromResult(Enumerable.Range(1, 20).Select(index => new WeatherForecast
            {
                Date = startDate.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            }).ToArray());
        }

        public Task<LazyLoadingDataSetResult<WeatherForecast>> GetTablePageData(
            ILazyLoadingOptions lazyLoadingOptions,
            IPagingOptions pageableOptions,
            ISortingOptions sortingOptions)
        {
            var startDate = DateTime.Now;
            var rng = new Random();
            var items = Enumerable.Range(1, 20)
                .Skip(pageableOptions.PageSize * pageableOptions.CurrentPage)
                .Take(pageableOptions.PageSize)
                .Select(index => new WeatherForecast
                {
                    Date = startDate.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                });

            items = string.IsNullOrEmpty(sortingOptions.SortExpression)
                 ? items
                 : items.AsQueryable().OrderBy(sortingOptions.SortExpression).ToList();

            return Task.FromResult(
                new LazyLoadingDataSetResult<WeatherForecast>
                {
                    Items = items.ToList(),
                    TotalCount = 100
                });
        }
    }
}
