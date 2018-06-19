using Blazor.FlexGrid.DataAdapters;
using Blazor.FlexGrid.DataSet;
using Blazor.FlexGrid.DataSet.Options;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.AspNetCore.Blazor.RenderTree;
using System.Threading.Tasks;

namespace Blazor.FlexGrid.Components
{
    [Route("/gridview")]
    public class GridView : BlazorComponent
    {
        private ITableDataSet tableDataSet;

        [Parameter]
        private ITableDataAdapter DataAdapter { get; set; }

        [Parameter]
        private ILazyLoadingOptions LazyLoadingOptions { get; set; }


        public GridView()
        {
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            var seq = 0;

            if (tableDataSet?.Items == null || tableDataSet.Items.Count <= 0)
            {
                builder.AddContent(++seq, "    ");
                builder.OpenElement(++seq, "p");
                builder.OpenElement(++seq, "em");
                builder.AddContent(++seq, "Loading...");
                builder.CloseElement();
                builder.CloseElement();
                builder.AddContent(++seq, "\n");
            }
            else
            {
                builder.OpenElement(seq, "table");
                builder.AddAttribute(++seq, "class", "table");
                builder.OpenElement(++seq, "tbody");

                foreach (var item in tableDataSet.Items)
                {
                    builder.OpenElement(++seq, "tr");
                    foreach (var property in item.GetType().GetProperties())
                    {
                        builder.OpenElement(++seq, "td");
                        builder.AddContent(++seq, property.GetValue(item).ToString());
                        builder.CloseElement();
                    }

                    builder.CloseElement();
                }

                builder.CloseElement();
                builder.CloseElement();
            }

            base.BuildRenderTree(builder);
        }

        protected override Task OnInitAsync()
        {
            tableDataSet = DataAdapter.GetTableDataSet(conf => conf.LazyLoadingOptions.DataUri = LazyLoadingOptions.DataUri);

            return tableDataSet.GoToPage(0);
        }
    }
}
