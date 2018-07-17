using Blazor.FlexGrid.Components.Configuration;
using Blazor.FlexGrid.Components.Configuration.MetaData;
using Blazor.FlexGrid.Components.Configuration.ValueFormatters;
using Blazor.FlexGrid.DataAdapters;
using Blazor.FlexGrid.DataSet;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.AspNetCore.Blazor.RenderTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Blazor.FlexGrid.Components.Renderers
{
    public class GridRendererContext
    {
        private int sequence = 0;
        private string firstColumnName;

        private readonly IEntityType gridEntityConfiguration;
        private readonly IPropertyValueAccessor propertyValueAccessor;
        private readonly RenderTreeBuilder renderTreeBuilder;
        private readonly IReadOnlyDictionary<string, ValueFormatter> valueFormatters;

        public string ActualColumnName { get; set; } = string.Empty;

        public bool IsFirstColumn => ActualColumnName.Equals(firstColumnName);

        public bool SortingByActualColumnName => TableDataSet.SortingOptions.SortExpression.Equals(ActualColumnName);

        public object ActualItem { get; set; }

        public IGridViewAnotations GridConfiguration { get; }

        public IReadOnlyCollection<PropertyInfo> GridItemProperties { get; }

        public ITableDataSet TableDataSet { get; }

        public IGridViewColumnAnotations ActualColumnConfiguration => gridEntityConfiguration.FindColumnConfiguration(ActualColumnName);

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

            GridItemProperties = imutableGridRendererContext.GridItemProperties;
            CssClasses = imutableGridRendererContext.CssClasses;
            GridConfiguration = new GridAnotations(imutableGridRendererContext.GridEntityConfiguration);
            TableDataSet = tableDataSet ?? throw new ArgumentNullException(nameof(tableDataSet));
            this.gridEntityConfiguration = imutableGridRendererContext.GridEntityConfiguration;
            this.propertyValueAccessor = imutableGridRendererContext.GetPropertyValueAccessor;
            this.valueFormatters = imutableGridRendererContext.ValueFormatters;
            this.renderTreeBuilder = renderTreeBuilder ?? throw new ArgumentNullException(nameof(renderTreeBuilder));
            this.firstColumnName = GridItemProperties.First().Name;
        }

        public void OpenElement(string elementName)
            => renderTreeBuilder.OpenElement(++sequence, elementName);

        public void CloseElement()
            => renderTreeBuilder.CloseElement();

        public void AddCssClass(string className)
            => renderTreeBuilder.AddAttribute(++sequence, HtmlAttributes.Class, className);

        public void AddOnClickEvent(Func<MulticastDelegate> onClickBindMethod)
            => renderTreeBuilder.AddAttribute(++sequence, HtmlJSEvents.OnClick, onClickBindMethod());

        public void AddContent(string content)
            => renderTreeBuilder.AddContent(++sequence, content);

        public void AddActualColumnValue()
            => renderTreeBuilder.AddContent(++sequence, valueFormatters[ActualColumnName]
                .FormatValue(propertyValueAccessor.GetValue(ActualItem, ActualColumnName)));

        public void AddDisabled(bool disabled)
            => renderTreeBuilder.AddAttribute(++sequence, HtmlAttributes.Disabled, disabled);

        public void AddColspan()
        {
            renderTreeBuilder.AddAttribute(++sequence, HtmlAttributes.Colspan, GridItemProperties.Count + 1);
            renderTreeBuilder.AddContent(++sequence, string.Empty);
        }

        public void OpenElement(string elementName, string className)
        {
            OpenElement(elementName);
            AddCssClass(className);
        }

        public void AddGridViewComponent(ITableDataAdapter tableDataAdapter)
        {
            renderTreeBuilder.OpenComponent<GridView>(++sequence);
            renderTreeBuilder.AddAttribute(++sequence, "DataAdapter", RuntimeHelpers.TypeCheck(tableDataAdapter));
            renderTreeBuilder.AddAttribute(++sequence, "PageSize", RuntimeHelpers.TypeCheck(5));
            renderTreeBuilder.CloseComponent();
        }
    }
}
