using Blazor.FlexGrid.Components.Configuration;
using Blazor.FlexGrid.Components.Configuration.Builders;
using Blazor.FlexGrid.Components.Renderers;
using Blazor.FlexGrid.Demo.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Blazor.FlexGrid.Demo.Client.GridConfigurations
{
    public class OrderGridConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.AllowInlineEdit();
            builder.HasEmptyItemsMessage("<h1>Test</h1>", true);
            builder.IsMasterTable();
            builder.HasDetailRelationship(o => o.OrderItems)
                .HasCaption("Order products");

            RenderFragment<Order> statusFragment = o => delegate (RenderTreeBuilder rendererTreeBuilder)
            {
                var internalBuilder = new BlazorRendererTreeBuilder(rendererTreeBuilder);
                internalBuilder
                    .OpenElement("div")
                    .AddContent($"{o.Status}!!!")
                    .CloseElement();

            };

            builder.Property(o => o.Status)
              .HasBlazorComponentValueRender(statusFragment);
        }
    }
}
