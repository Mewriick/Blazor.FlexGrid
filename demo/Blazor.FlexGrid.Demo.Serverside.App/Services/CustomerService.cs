using Blazor.FlexGrid.Demo.Shared;
using System.Collections.Generic;
using System.Linq;

namespace Blazor.FlexGrid.Demo.Serverside.App.Services
{
    public class CustomerService
    {
        public ICollection<Customer> Customers()
        {
            return Enumerable.Range(1, 20).Select(index =>
                 new Customer
                 {
                     Id = index,
                     Email = $"email{index}@email.com",
                     Country = "GB",
                     Name = "John",
                     SurName = "Doe",
                     Address = "London"
                 }).ToList();
        }

        public ICollection<CustomerAddress> CustomersAddresses()
        {
            return Enumerable.Range(1, 20).Select(index =>
                 new CustomerAddress
                 {
                     City = "London",
                     CustomerId = index,
                     Number = $"22 - {index}",
                     Street = "Baker Street"
                 }).ToList();
        }
    }
}
