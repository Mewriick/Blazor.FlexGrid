using Blazor.FlexGrid.DataSet;
using Blazor.FlexGrid.DataSet.Options;
using Blazor.FlexGrid.Demo.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Blazor.Components.Demo.FlexGrid.Services
{
    public class WeatherForecastService : ILazyDataSetLoader<WeatherForecast>, ILazyDataSetItemManipulator<WeatherForecast>
    {
        private static string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        private readonly StaticRepositoryCollections staticRepositoryCollections;

        public WeatherForecastService(StaticRepositoryCollections staticRepositoryCollections)
        {
            this.staticRepositoryCollections = staticRepositoryCollections;
        }

        public Task<WeatherForecast> DeleteItem(WeatherForecast item, ILazyLoadingOptions lazyLoadingOptions)
        {
            if (staticRepositoryCollections.Forecasts.TryRemove(item.Id, out var value))
            {
                return Task.FromResult(value);
            }

            return Task.FromResult(default(WeatherForecast));
        }

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
            var items = staticRepositoryCollections.Forecasts.Values.AsQueryable();

            var sortExp = sortingOptions?.SortExpression;
            if (!string.IsNullOrEmpty(sortExp))
            {
                if (sortingOptions.SortDescending)
                {
                    sortExp += " descending";
                }
                items = items.OrderBy(sortExp);
            }

            items = items
                .Skip(pageableOptions.PageSize * pageableOptions.CurrentPage)
                .Take(pageableOptions.PageSize);

            return Task.FromResult(new LazyLoadingDataSetResult<WeatherForecast>
            {
                Items = items.ToList(),
                TotalCount = staticRepositoryCollections.Forecasts.Count
            });
        }

		public Task<LazyLoadingDataSetResult<WeatherForecast>> GetGroupedTablePageData(
            ILazyLoadingOptions lazyLoadingOptions, IPagingOptions pageableOptions, 
            ISortingOptions sortingOptions, IGroupingOptions groupingOptions)
        {

            try
            {
                var items = staticRepositoryCollections.Forecasts.Values.AsQueryable();


                var param = Expression.Parameter(typeof(WeatherForecast));
                var property = Expression.PropertyOrField(param, groupingOptions.GroupedProperty.Name);
                
                var keyPropertyConstructors = typeof(KeyProperty).GetConstructors();
                var newExpr = Expression.New(keyPropertyConstructors.FirstOrDefault(c => c.GetParameters()[0].ParameterType == property.Type)
                    , property);
                var lambda = Expression.Lambda<Func<WeatherForecast, KeyProperty>>(newExpr, param);
                var groupedItems = items.GroupBy(lambda)
                        .Select(grp => new GroupItem<WeatherForecast>(grp.Key.Key, grp.ToList()));
                 




                var groupedItemsAfterPaging = groupedItems
                    .Skip(pageableOptions.PageSize * pageableOptions.CurrentPage)
                    .Take(pageableOptions.PageSize);

                var groupedItemsAfterSorting = new List<GroupItem<WeatherForecast>>();
                var sortExp = sortingOptions?.SortExpression;
                if (!string.IsNullOrEmpty(sortExp))
                {
                    if (sortingOptions.SortDescending)
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


                return Task.FromResult(new LazyLoadingDataSetResult<WeatherForecast>()
                {
                    Items = groupedItemsAfterSorting.SelectMany(grp => grp.Items).ToList(),
                    TotalCount = groupedItems.Count()
                });

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

		
        public Task<WeatherForecast> SaveItem(WeatherForecast item, ILazyLoadingOptions lazyLoadingOptions)
        {
            var id = item.Id;
            if (staticRepositoryCollections.Forecasts.TryGetValue(id, out var value))
            {
                if (staticRepositoryCollections.Forecasts.TryUpdate(id, item, value))
                {
                    // Update Success
                    return Task.FromResult(item);
                }
            }
            else if (staticRepositoryCollections.Forecasts.TryAdd(id, item))
            {
                // Create Success
                return Task.FromResult(item);
            }

            // Conflict
            return Task.FromResult(default(WeatherForecast));
        }
		
		
    }
	
	        public class KeyProperty : IEquatable<KeyProperty>
        {
            public object Key { get; set; }

            public KeyProperty(object key)
            {
                this.Key = (object)key;
            }

            public KeyProperty(int key)
            {
                this.Key = (object)key;
            }

            public KeyProperty(String key)
            {
                this.Key = (object)key;
            }

            public KeyProperty(DateTime key)
            {
                this.Key = (object)key;
            }

            public bool Equals(KeyProperty other)
            {
                return this.Key == other.Key;
            }

            public override bool Equals(object other)
            {
                return this.Key == ((KeyProperty)other).Key;
            }

            public override int GetHashCode()
            {
                return Key.GetHashCode();
            }
       }
}
