using Blazor.FlexGrid.Components.Configuration;
using Blazor.FlexGrid.Components.Configuration.MetaData;
using Blazor.FlexGrid.Components.Configuration.ValueFormatters;
using Blazor.FlexGrid.DataAdapters;
using Blazor.FlexGrid.DataSet;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.AspNetCore.Blazor.RenderTree;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Blazor.FlexGrid.Components.Renderers
{
    public class GridRendererContext
    {
        private int sequence = 0;
        private readonly IPropertyValueAccessor propertyValueAccessor;
        private readonly IReadOnlyDictionary<string, ValueFormatter> valueFormatters;

        public string ActualColumnName { get; set; } = string.Empty;

        public bool SortingByActualColumnName => TableDataSet.SortingOptions.SortExpression.Equals(ActualColumnName);

        public object ActualItem { get; set; }

        public IEntityType GridConfiguration { get; }

        public IReadOnlyCollection<PropertyInfo> GridItemProperties { get; }

        public RenderTreeBuilder RenderTreeBuilder { get; }

        public ITableDataSet TableDataSet { get; }

        public IGridViewColumnAnotations ActualColumnConfiguration => GridConfiguration.FindColumnConfiguration(ActualColumnName);

        public GridCssClasses CssClasses { get; }


        public GridRendererContext(
            ImutableGridRendererContext imutableGridRendererContext,
            RenderTreeBuilder renderTreeBuilder,
            ITableDataSet tableDataSet)
        {
            if (imutableGridRendererContext is null)
            {
                throw new ArgumentNullException(nameof(imutableGridRendererContext));
            }

            GridConfiguration = imutableGridRendererContext.GridConfiguration;
            GridItemProperties = imutableGridRendererContext.GridItemProperties;
            propertyValueAccessor = imutableGridRendererContext.GetPropertyValueAccessor;
            valueFormatters = imutableGridRendererContext.ValueFormatters;
            CssClasses = imutableGridRendererContext.CssClasses;
            RenderTreeBuilder = renderTreeBuilder ?? throw new ArgumentNullException(nameof(renderTreeBuilder));
            TableDataSet = tableDataSet ?? throw new ArgumentNullException(nameof(tableDataSet));
        }

        public void OpenElement(string elementName)
            => RenderTreeBuilder.OpenElement(++sequence, elementName);

        public void CloseElement()
            => RenderTreeBuilder.CloseElement();

        public void AddCssClass(string className)
            => RenderTreeBuilder.AddAttribute(++sequence, HtmlAttributes.Class, className);

        public void AddOnClickEvent(Func<MulticastDelegate> onClickBindMethod)
            => RenderTreeBuilder.AddAttribute(++sequence, HtmlJSEvents.OnClick, onClickBindMethod());

        public void AddContent(string content)
            => RenderTreeBuilder.AddContent(++sequence, content);

        public void AddActualColumnValue()
            => RenderTreeBuilder.AddContent(++sequence, valueFormatters[ActualColumnName]
                .FormatValue(propertyValueAccessor.GetValue(ActualItem, ActualColumnName)));

        public void AddDisabled(bool disabled)
            => RenderTreeBuilder.AddAttribute(++sequence, HtmlAttributes.Disabled, disabled);

        public void OpenElement(string elementName, string className)
        {
            OpenElement(elementName);
            AddCssClass(className);
        }

        public void AddGridViewComponent(ITableDataAdapter tableDataAdapter)
        {
            RenderTreeBuilder.OpenComponent<GridView>(++sequence);
            RenderTreeBuilder.AddAttribute(++sequence, "DataAdapter", RuntimeHelpers.TypeCheck(tableDataAdapter));
            RenderTreeBuilder.AddAttribute(++sequence, "PageSize", RuntimeHelpers.TypeCheck(5));
            RenderTreeBuilder.CloseComponent();
        }
    }
}
