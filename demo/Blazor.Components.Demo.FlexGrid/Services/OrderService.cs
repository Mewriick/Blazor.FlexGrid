using Blazor.FlexGrid.DataSet;
using Blazor.FlexGrid.DataSet.Options;
using Blazor.FlexGrid.Demo.Shared;
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

        public Task<LazyLoadingDataSetResult<GroupItem<Order>>> GetGroupedTablePageData(ILazyLoadingOptions lazyLoadingOptions, IPagingOptions pageableOptions, ISortingOptions sortingOptions, IGroupingOptions groupingOptions)
        {
            throw new NotImplementedException();
        }

        public Task<LazyLoadingDataSetResult<Order>> GetTablePageData(
            ILazyLoadingOptions lazyLoadingOptions,
            IPagingOptions pageableOptions,
            ISortingOptions sortingOptions)
        {
            var customerId = Convert.ToInt32(lazyLoadingOptions.RequestParams.First(e => e.Key == "CustomerId").Value);
            var customerOrders = orders.Where(o => o.CustomerId == customerId);
            var pageableCustomerOrders = customerOrders
                .Skip(pageableOptions.PageSize * pageableOptions.CurrentPage)
                .Take(pageableOptions.PageSize)
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
