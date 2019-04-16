using Blazor.FlexGrid.DataSet;
using Blazor.FlexGrid.DataSet.Http;
using Blazor.FlexGrid.Demo.Shared;
using Blazor.FlexGrid.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.Components.Demo.FlexGrid.Services
{
    public class OrderService : ILazyDataSetLoader<Order>
    {
        private List<Order> orders;

        public OrderService()
        {
            var random = new Random();
            orders = Enumerable.Range(1, 1000).Select(index =>
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
        }

        public Task<LazyLoadingDataSetResult<GroupItem<Order>>> GetGroupedTablePageData(
            RequestOptions requestOptions,
            IReadOnlyCollection<IFilterDefinition> filterDefinitions = null)
        {
            var emptyResult = new LazyLoadingDataSetResult<GroupItem<Order>>
            {
                Items = Enumerable.Empty<GroupItem<Order>>().ToList()
            };

            return Task.FromResult(emptyResult);
        }

        public Task<LazyLoadingDataSetResult<Order>> GetTablePageData(
            RequestOptions requestOptions,
            IReadOnlyCollection<IFilterDefinition> filterDefinitions = null)
        {
            var customerId = Convert.ToInt32(requestOptions.LazyLoadingOptions.RequestParams.First(e => e.Key == "CustomerId").Value);
            var customerOrders = orders.Where(o => o.CustomerId == customerId);
            var pageableCustomerOrders = customerOrders
                .Skip(requestOptions.PageableOptions.PageSize * requestOptions.PageableOptions.CurrentPage)
                .Take(requestOptions.PageableOptions.PageSize)
                .ToList();

            return Task.FromResult(new LazyLoadingDataSetResult<Order>
            {
                Items = pageableCustomerOrders,
                TotalCount = customerOrders.Count()
            });
        }

        public ICollection<Order> Orders() => orders;
    }
}
