using Blazor.FlexGrid.Components.Configuration;
using Blazor.FlexGrid.Components.Configuration.MetaData;
using Blazor.FlexGrid.Components.Configuration.ValueFormatters;
using Blazor.FlexGrid.Components.Filters;
using Blazor.FlexGrid.DataAdapters;
using Blazor.FlexGrid.DataSet;
using Blazor.FlexGrid.DataSet.Options;
using Blazor.FlexGrid.Permission;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Blazor.FlexGrid.Components.Renderers
{
    public class GridRendererContext : IActualItemContext<object>
    {
        private string firstColumnName;
        private string lastColumnName;
        private readonly IEntityType gridEntityConfiguration;
        private readonly IReadOnlyDictionary<string, IValueFormatter> valueFormatters;
        private readonly IReadOnlyDictionary<string, IRenderFragmentAdapter> specialColumnValues;

        public string ActualColumnName { get; set; } = string.Empty;

        public bool ActualColumnPropertyCanBeEdited { get; set; }

        public bool IsFirstColumn => ActualColumnName.Equals(firstColumnName);

        public bool IsLastColumn => ActualColumnName.Equals(lastColumnName);

        public int NumberOfColumns { get; }

        public bool IsActualItemEdited => TableDataSet.IsItemEdited(ActualItem);

        public bool SortingByActualColumnName => TableDataSet.SortingOptions.SortExpression.Equals(ActualColumnName);

        public object ActualItem { get; set; }

        public IGridViewAnotations GridConfiguration { get; }

        public IReadOnlyCollection<PropertyInfo> GridItemProperties { get; }

        public IReadOnlyCollection<PropertyInfo> GridItemCollectionProperties { get; }

        public ITableDataSet TableDataSet { get; }

        public IGridViewColumnAnotations ActualColumnConfiguration => gridEntityConfiguration.FindColumnConfiguration(ActualColumnName);

        public GridCssClasses CssClasses { get; }

        public ITypePropertyAccessor PropertyValueAccessor { get; }

        public IRendererTreeBuilder RendererTreeBuilder { get; }

        public FlexGridContext FlexGridContext { get; }

        public Action RequestRerenderNotification => FlexGridContext.RequestRerenderTableRowsNotification;

        public GridRendererContext(
            ImutableGridRendererContext imutableGridRendererContext,
            IRendererTreeBuilder rendererTreeBuilder,
            ITableDataSet tableDataSet,
            FlexGridContext flexGridContext)
        {
            if (imutableGridRendererContext is null)
            {
                throw new ArgumentNullException(nameof(imutableGridRendererContext));
            }

            RendererTreeBuilder = rendererTreeBuilder ?? throw new ArgumentNullException(nameof(RendererTreeBuilder));
            TableDataSet = tableDataSet ?? throw new ArgumentNullException(nameof(tableDataSet));
            FlexGridContext = flexGridContext ?? throw new ArgumentNullException(nameof(flexGridContext));

            GridConfiguration = imutableGridRendererContext.GridConfiguration;
            GridItemProperties = imutableGridRendererContext.GridItemProperties;
            GridItemCollectionProperties = imutableGridRendererContext.GridEntityConfiguration.ClrTypeCollectionProperties;
            CssClasses = imutableGridRendererContext.CssClasses;
            PropertyValueAccessor = imutableGridRendererContext.GetPropertyValueAccessor;

            this.gridEntityConfiguration = imutableGridRendererContext.GridEntityConfiguration;
            this.valueFormatters = imutableGridRendererContext.ValueFormatters;
            this.specialColumnValues = imutableGridRendererContext.SpecialColumnValues;
            this.firstColumnName = GridItemProperties.First().Name;
            this.lastColumnName = GridItemProperties.Last().Name;
            NumberOfColumns = GridItemProperties.Count +
                (imutableGridRendererContext.InlineEditItemIsAllowed() || imutableGridRendererContext.CreateItemIsAllowed() ? 1 : 0) +
                (GridConfiguration.IsMasterTable ? 1 : 0);
        }

        public void OpenElement(string elementName)
            => RendererTreeBuilder.OpenElement(elementName);

        public void CloseElement()
            => RendererTreeBuilder.CloseElement();

        public void AddCssClass(string className)
            => RendererTreeBuilder.AddAttribute(HtmlAttributes.Class, className);

        public void AddHeaderStyle(string style)
            => RendererTreeBuilder.AddAttribute(HtmlAttributes.Style, style);

        public void AddOnClickEvent(Func<MulticastDelegate> onClickBindMethod)
            => RendererTreeBuilder.AddAttribute(HtmlJSEvents.OnClick, onClickBindMethod());

        public void AddOnChangeEvent(Func<MulticastDelegate> onClickBindMethod)
            => RendererTreeBuilder.AddAttribute(HtmlJSEvents.OnChange, onClickBindMethod());

        public void AddContent(string content)
            => RendererTreeBuilder.AddContent(content);

        public void AddMarkupContent(string content)
            => RendererTreeBuilder.AddContent(new MarkupString(content));

        public void AddActualColumnValue(PermissionContext permissionContext)
        {
            if (!permissionContext.HasCurrentUserReadPermission(ActualColumnName))
            {
                RendererTreeBuilder.AddContent("*****");
                return;
            }

            if (specialColumnValues.TryGetValue(ActualColumnName, out var rendererFragmentAdapter))
            {
                var fragment = rendererFragmentAdapter.GetColumnFragment(ActualItem);
                RendererTreeBuilder.AddContent(fragment);
                return;
            }

            var valueFormatter = valueFormatters[ActualColumnName];
            var inputForColumnValueFormatter = valueFormatter.FormatterType == ValueFormatterType.SingleProperty
                ? PropertyValueAccessor.GetValue(ActualItem, ActualColumnName)
                : ActualItem;

            RendererTreeBuilder.AddContent(new MarkupString(
               valueFormatter.FormatValue(inputForColumnValueFormatter))
            );
        }

        public void AddDisabled(bool disabled)
            => RendererTreeBuilder.AddAttribute(HtmlAttributes.Disabled, disabled);

        public void AddColspan()
        {
            RendererTreeBuilder.AddAttribute(HtmlAttributes.Colspan, GridItemProperties.Count + 1);
            RendererTreeBuilder.AddContent(string.Empty);
        }

        public void OpenElement(string elementName, string className, string style = null)
        {
            OpenElement(elementName);
            AddCssClass(className);
            if (!string.IsNullOrEmpty(style))
            {
                AddHeaderStyle(style);
            }
        }

        public void AddAttribute(string name, object value)
            => RendererTreeBuilder.AddAttribute(name, value);

        public void AddAttribute(string name, Action<UIEventArgs> value)
            => RendererTreeBuilder.AddAttribute(name, value);

        public void AddDetailGridViewComponent(ITableDataAdapter tableDataAdapter)
        {
            if (tableDataAdapter is null)
            {
                return;
            }

            var masterDetailRelationship = GridConfiguration.FindRelationshipConfiguration(tableDataAdapter.UnderlyingTypeOfItem);
            var pageSize = RuntimeHelpers.TypeCheck(masterDetailRelationship.DetailGridViewPageSize(TableDataSet));

            RendererTreeBuilder.OpenComponent(typeof(GridViewGeneric<>).MakeGenericType(tableDataAdapter.UnderlyingTypeOfItem));
            RendererTreeBuilder.AddAttribute("DataAdapter", RuntimeHelpers.TypeCheck(tableDataAdapter));
            RendererTreeBuilder.AddAttribute(nameof(ITableDataSet.PageableOptions.PageSize), pageSize);

            RendererTreeBuilder.AddAttribute(
                nameof(ILazyTableDataSet.LazyLoadingOptions),
                new LazyLoadingOptions
                {
                    DataUri = masterDetailRelationship.DetailGridLazyLoadingUrl(),
                    PutDataUri = masterDetailRelationship.DetailGridUpdateUrl(),
                    DeleteUri = masterDetailRelationship.DetailGridDeleteUrl()
                });

            AddEventAttributes();
            RendererTreeBuilder.CloseComponent();
        }

        public void AddGridViewComponent(ITableDataAdapter tableDataAdapter)
        {
            RendererTreeBuilder.OpenComponent(typeof(GridViewGroup<>).MakeGenericType(tableDataAdapter.UnderlyingTypeOfItem));
            RendererTreeBuilder.AddAttribute("DataAdapter", RuntimeHelpers.TypeCheck(tableDataAdapter));
            RendererTreeBuilder.AddAttribute(nameof(ITableDataSet.PageableOptions.PageSize), GridConfiguration.GroupingOptions.GroupPageSize);
            RendererTreeBuilder.CloseComponent();
        }

        public void AddFilterComponent(PropertyInfo property)
        {
            RendererTreeBuilder
                .OpenComponent(typeof(ColumnFilter<>).MakeGenericType(property.PropertyType))
                .AddAttribute("ColumnName", property.Name)
                .CloseComponent();
        }

        public object GetActualItemColumnValue(string columnName)
            => PropertyValueAccessor.GetValue(ActualItem, columnName);

        public void SetActualItemColumnValue(string columnName, object value)
            => PropertyValueAccessor.SetValue(ActualItem, columnName, value);

        public string GetColumnCaption(string columnName)
            => gridEntityConfiguration.FindColumnConfiguration(columnName)?.Caption;

        private void AddEventAttributes()
        {
            if (TableDataSet.GridViewEvents.SaveOperationFinished != null)
            {
                RendererTreeBuilder.AddAttribute(
                    nameof(ITableDataSet.GridViewEvents.SaveOperationFinished),
                    RuntimeHelpers.TypeCheck(TableDataSet.GridViewEvents.SaveOperationFinished));
            }

            if (TableDataSet.GridViewEvents.NewItemCreated != null)
            {
                RendererTreeBuilder.AddAttribute(
                    nameof(ITableDataSet.GridViewEvents.NewItemCreated),
                    RuntimeHelpers.TypeCheck(TableDataSet.GridViewEvents.NewItemCreated));
            }

            if (TableDataSet.GridViewEvents.OnItemClicked != null)
            {
                RendererTreeBuilder.AddAttribute(
                    nameof(ITableDataSet.GridViewEvents.OnItemClicked),
                    RuntimeHelpers.TypeCheck(TableDataSet.GridViewEvents.OnItemClicked));
            }

            if (TableDataSet.GridViewEvents.DeleteOperationFinished != null)
            {
                RendererTreeBuilder.AddAttribute(
                    nameof(ITableDataSet.GridViewEvents.DeleteOperationFinished),
                    RuntimeHelpers.TypeCheck(TableDataSet.GridViewEvents.DeleteOperationFinished));
            }
        }
    }
}
