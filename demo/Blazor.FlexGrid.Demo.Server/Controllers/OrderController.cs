using Blazor.FlexGrid.Demo.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Blazor.FlexGrid.Demo.Server.Controllers
{
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        [HttpGet("[action]")]
        public IActionResult Orders()
        {
            var random = new Random();

            return Ok(Enumerable.Range(1, 100).Select(index =>
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
                            Id = 1,
                            Name = "Test",
                            OrderId = index,
                            Price = 100
                        }
                    }
                })
            );
        }
    }
}
