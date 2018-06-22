using Blazor.FlexGrid.Components.Configuration;
using Blazor.FlexGrid.Components.Renderers;
using Blazor.FlexGrid.DataAdapters;
using Blazor.FlexGrid.DataSet;
using Blazor.FlexGrid.DataSet.Options;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.AspNetCore.Blazor.RenderTree;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.FlexGrid.Components
{
    [Route("/gridview")]
    public class GridView : BlazorComponent
    {
        private ITableDataSet tableDataSet;

        [Inject]
        private IGridComponentsContext ConfigurationContext { get; set; }

        [Inject]
        private IEnumerable<IGridRenderer> Renderers { get; set; }

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
                var itemType = tableDataSet.GetType().GenericTypeArguments[0];
                var rendererContext = new GridRendererContext(
                        ConfigurationContext.FindGridConfigurationByType(itemType),
                        itemType.GetProperties().ToList(),
                        builder,
                        tableDataSet
                    );

                builder.OpenElement(seq, "table");
                builder.AddAttribute(++seq, "class", "table");

                foreach (var renderer in Renderers)
                    renderer.Render(rendererContext);

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
