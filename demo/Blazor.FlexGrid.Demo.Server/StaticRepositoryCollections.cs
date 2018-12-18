using Blazor.FlexGrid.Demo.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blazor.FlexGrid.Demo.Server
{
    public class StaticRepositoryCollections
    {
        private static string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public List<Order> Orders { get; }

        public List<WeatherForecast> Forecasts { get; }

        public StaticRepositoryCollections()
        {
            var random = new Random();
            Orders = Enumerable.Range(1, 1000).Select(index =>
                new Order
                {
                    Id = index,
                    CustomerId = random.Next(1, 20),
                    OrderDate = DateTimeOffset.Now,
                    Status = "New",
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

            Forecasts = Enumerable.Range(1, 100).Select(index =>
                        new WeatherForecast
                        {
                            Date = DateTime.Now.AddDays(index),
                            TemperatureC = random.Next(-20, 55),
                            Summary = Summaries[random.Next(Summaries.Length)]

                        }).ToList();
        }
    }
}
