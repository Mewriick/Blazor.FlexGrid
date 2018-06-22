using Blazor.FlexGrid.Components.Configuration;
using Blazor.FlexGrid.Components.Configuration.MetaData;
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

        [Inject]
        private IGridComponentsContext ConfigurationContext { get; set; }

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
                var gridConfiguration = ConfigurationContext.FindGridConfigurationByType(itemType);
                var itemTypeProperties = itemType.GetProperties();

                builder.OpenElement(seq, "table");
                builder.AddAttribute(++seq, "class", "table");

                builder.OpenElement(++seq, "thead");
                builder.OpenElement(++seq, "tr");
                foreach (var property in itemTypeProperties)
                {
                    builder.OpenElement(++seq, "th");

                    var columnConfiguration = gridConfiguration.FindProperty(property.Name);
                    if (columnConfiguration != null)
                    {
                        var columnCaption = columnConfiguration[GridViewAnnotationNames.ColumnCaption] as Annotation;
                        if (columnCaption is null)
                        {
                            builder.AddContent(++seq, property.Name);
                        }
                        else
                        {
                            builder.AddContent(++seq, columnCaption.Value.ToString());
                        }
                    }
                    else
                    {

                        builder.AddContent(++seq, property.Name);
                    }

                    builder.CloseElement();
                }

                builder.CloseElement();
                builder.CloseElement();

                builder.OpenElement(++seq, "tbody");
                foreach (var item in tableDataSet.Items)
                {
                    builder.OpenElement(++seq, "tr");
                    foreach (var property in itemTypeProperties)
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
