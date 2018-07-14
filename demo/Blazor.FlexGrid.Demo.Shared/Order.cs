using System;

namespace Blazor.FlexGrid.Demo.Shared
{
    public class Order
    {
        public int Id { get; set; }

        public DateTimeOffset OrderDate { get; set; }

        public string Status { get; set; }

        public int CustomerId { get; set; }
    }
}
