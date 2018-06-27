using Blazor.FlexGrid.Components.Configuration;
using Blazor.FlexGrid.Components.Configuration.MetaData;
using Blazor.FlexGrid.Components.Configuration.ValueFormatters;
using Blazor.FlexGrid.DataSet;
using Microsoft.AspNetCore.Blazor.RenderTree;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Blazor.FlexGrid.Components.Renderers
{
    public class GridRendererContext
    {
        private readonly IPropertyValueAccessor propertyValueAccessor;
        private readonly IReadOnlyDictionary<string, ValueFormatter> valueFormatters;

        public int Sequence { get; set; }

        public string ActualColumnName { get; set; } = string.Empty;

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

            Sequence = 0;
            GridConfiguration = imutableGridRendererContext.GridConfiguration;
            GridItemProperties = imutableGridRendererContext.GridItemProperties;
            propertyValueAccessor = imutableGridRendererContext.GetPropertyValueAccessor;
            valueFormatters = imutableGridRendererContext.ValueFormatters;
            CssClasses = imutableGridRendererContext.CssClasses;
            RenderTreeBuilder = renderTreeBuilder ?? throw new ArgumentNullException(nameof(renderTreeBuilder));
            TableDataSet = tableDataSet ?? throw new ArgumentNullException(nameof(tableDataSet));
        }

        public void OpenElement(string elementName)
            => RenderTreeBuilder.OpenElement(++Sequence, elementName);

        public void CloseElement()
            => RenderTreeBuilder.CloseElement();

        public void AddCssClass(string className)
            => RenderTreeBuilder.AddAttribute(++Sequence, HtmlAttributes.Class, className);

        public void AddOnClickEvent(Func<MulticastDelegate> onClickBindMethod)
            => RenderTreeBuilder.AddAttribute(++Sequence, HtmlJSEvents.OnClick, onClickBindMethod());

        public void AddContent(string content)
            => RenderTreeBuilder.AddContent(++Sequence, content);

        public void AddActualColumnValue()
            => RenderTreeBuilder.AddContent(++Sequence, valueFormatters[ActualColumnName]
                .FormatValue(propertyValueAccessor.GetValue(ActualItem, ActualColumnName)));

        public void AddDisabled(bool disabled)
            => RenderTreeBuilder.AddAttribute(++Sequence, HtmlAttributes.Disabled, disabled);

        public void OpenElement(string elementName, string className)
        {
            OpenElement(elementName);
            AddCssClass(className);
        }
    }
}
