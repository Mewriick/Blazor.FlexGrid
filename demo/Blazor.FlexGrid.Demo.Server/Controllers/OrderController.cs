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
            return Ok(Enumerable.Range(1, 20).Select(index =>
                new Order
                {
                    Id = index,
                    CustomerId = index,
                    OrderDate = DateTimeOffset.Now,
                    Status = "New"
                })
            );
        }
    }
}
