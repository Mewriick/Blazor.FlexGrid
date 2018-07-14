

using Blazor.FlexGrid.Demo.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Blazor.FlexGrid.Demo.Server.Controllers
{
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {
        [HttpGet("[action]")]
        public IActionResult Customers()
        {
            return Ok(Enumerable.Range(1, 20).Select(index =>
                 new Customer
                 {
                     Id = index,
                     Email = $"email{index}@email.com",
                     Country = "GB",
                     Name = "John",
                     SurName = "Doe",
                     Address = "London"
                 })
            );
        }
    }
}
