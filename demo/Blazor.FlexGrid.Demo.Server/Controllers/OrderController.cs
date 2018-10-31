using Blazor.FlexGrid.Demo.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Blazor.FlexGrid.Demo.Server.Controllers
{
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        private readonly StaticRepositoryCollections staticRepositoryCollections;

        public OrderController(StaticRepositoryCollections staticRepositoryCollections)
        {
            this.staticRepositoryCollections = staticRepositoryCollections;
        }

        [HttpGet("[action]")]
        public IActionResult Orders(int customerId, int pageNumber, int pageSize, SortingParams sortingParams)
        {
            var customerOrders = staticRepositoryCollections.Orders.Where(o => o.CustomerId == customerId);
            var pageableCustomerOrders = customerOrders
                .Skip(pageSize * pageNumber)
                .Take(pageSize)
                .ToList();

            return Ok(new
            {
                Items = pageableCustomerOrders,
                TotalCount = customerOrders.Count()
            });
        }
    }
}
