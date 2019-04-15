using Blazor.FlexGrid.Components.Configuration;
using Blazor.FlexGrid.Components.Configuration.Builders;
using Blazor.FlexGrid.Demo.Shared;

namespace Blazor.Components.Demo.FlexGrid.GridConfigurations
{
    public class OrderGridConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.AllowInlineEdit();

            builder.IsMasterTable();
            builder.HasDetailRelationship(o => o.OrderItems)
                .HasCaption("Order products");

            builder.EnableGrouping();
        }
    }
}
