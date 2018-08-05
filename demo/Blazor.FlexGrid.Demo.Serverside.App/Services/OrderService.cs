using Blazor.FlexGrid.Demo.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blazor.FlexGrid.Demo.Serverside.App.Services
{
    public class OrderService
    {
        public ICollection<Order> Orders()
        {
            var random = new Random();

            return Enumerable.Range(1, 100).Select(index =>
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
        }
    }
}
