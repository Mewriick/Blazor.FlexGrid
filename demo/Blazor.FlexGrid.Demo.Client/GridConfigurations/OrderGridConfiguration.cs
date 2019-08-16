using Blazor.FlexGrid.Components.Configuration;
using Blazor.FlexGrid.Components.Configuration.Builders;
using Blazor.FlexGrid.Demo.Shared;

namespace Blazor.FlexGrid.Demo.Client.GridConfigurations
{
    public class OrderGridConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.AllowInlineEdit();
            builder.HasEmptyItemsMessage("<h1>Test</h1>");
            builder.IsMasterTable();
            builder.HasDetailRelationship(o => o.OrderItems)
                .HasCaption("Order products");
        }
    }
}
