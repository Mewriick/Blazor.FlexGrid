using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Blazor.FlexGrid.Demo.Shared;

namespace Blazor.FlexGrid.Demo.Server
{
    public class StaticRepositoryCollections
    {
        private static string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public List<Order> Orders { get; }

        public ConcurrentDictionary<int, WeatherForecast> Forecasts { get; }

        public StaticRepositoryCollections()
        {
            var random = new Random();
            Orders = Enumerable.Range(1, 1000).Select(index =>
                new Order
                {
                    Id = index,
                    CustomerId = random.Next(1, 20),
                    OrderDate = DateTimeOffset.Now,
                    Status = OrderState.New,
                    OrderItems = new System.Collections.Generic.List<OrderItem>
                    {
                        new OrderItem
                        {
                            Id = index + 1,
                            Name = "Test",
                            OrderId = index,
                            Price = 100
                        }
                    }
                }).ToList();

            Forecasts = Enumerable.Range(1, 100)
                .Select(index => new WeatherForecast {
                    Id = index,
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = random.Next(-20, 55),
                    Summary = Summaries[random.Next(Summaries.Length)]
                }).ToConcurrentDictionary(e => e.Id);
        }
    }

    internal static class EnumerableExtensions
    {
        public static ConcurrentDictionary<TKey, TValue> ToConcurrentDictionary<TKey, TValue>(this IEnumerable<TValue> source, Func<TValue, TKey> keySelector)
            => new ConcurrentDictionary<TKey, TValue>(source.Select(e => KeyValuePair.Create(keySelector(e), e)));
    }
}
