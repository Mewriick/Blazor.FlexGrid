using Blazor.FlexGrid.Components.Configuration;
using Blazor.FlexGrid.Components.Configuration.Builders;
using Blazor.FlexGrid.Demo.Shared;

namespace Blazor.FlexGrid.Demo.Client.GridConfigurations
{
    public class CustomerGridConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.IsMasterTable()
                .HasDetailRelationship<Order>(c => c.Id, o => o.CustomerId);

            builder.HasDetailRelationship<CustomerAddress>(c => c.Id, o => o.CustomerId);
        }
    }
}
