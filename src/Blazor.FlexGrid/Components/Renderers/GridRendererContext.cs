using Blazor.FlexGrid.Components.Configuration;
using Blazor.FlexGrid.Components.Configuration.MetaData;
using Blazor.FlexGrid.Components.Configuration.ValueFormatters;
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
        private readonly IReadOnlyDictionary<string, IValueFormatter<object>> valueFormatters;
        private readonly IReadOnlyDictionary<string, IRenderFragmentAdapter<object>> specialColumnValues;

        public string ActualColumnName { get; set; } = string.Empty;

        public bool ActualColumnPropertyCanBeEdited { get; set; }

        public bool IsFirstColumn => ActualColumnName.Equals(firstColumnName);

        public bool IsLastColumn => ActualColumnName.Equals(lastColumnName);

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

        public GridRendererContext(
            ImutableGridRendererContext imutableGridRendererContext,
            IRendererTreeBuilder rendererTreeBuilder,
            ITableDataSet tableDataSet)
        {
            if (imutableGridRendererContext is null)
            {
                throw new ArgumentNullException(nameof(imutableGridRendererContext));
            }

            GridConfiguration = new GridAnotations(imutableGridRendererContext.GridEntityConfiguration);
            GridItemProperties = imutableGridRendererContext.GridItemProperties;
            GridItemCollectionProperties = imutableGridRendererContext.GridEntityConfiguration.ClrTypeCollectionProperties;
            CssClasses = GridConfiguration.CssClasses;
            TableDataSet = tableDataSet ?? throw new ArgumentNullException(nameof(tableDataSet));
            PropertyValueAccessor = imutableGridRendererContext.GetPropertyValueAccessor;
            RendererTreeBuilder = rendererTreeBuilder ?? throw new ArgumentNullException(nameof(RendererTreeBuilder));

            this.gridEntityConfiguration = imutableGridRendererContext.GridEntityConfiguration;
            this.valueFormatters = imutableGridRendererContext.ValueFormatters;
            this.specialColumnValues = imutableGridRendererContext.SpecialColumnValues;
            this.firstColumnName = GridItemProperties.First().Name;
            this.lastColumnName = GridItemProperties.Last().Name;
        }

        public void OpenElement(string elementName)
            => RendererTreeBuilder.OpenElement(elementName);

        public void CloseElement()
            => RendererTreeBuilder.CloseElement();

        public void AddCssClass(string className)
            => RendererTreeBuilder.AddAttribute(HtmlAttributes.Class, className);

        public void AddOnClickEvent(Func<MulticastDelegate> onClickBindMethod)
            => RendererTreeBuilder.AddAttribute(HtmlJSEvents.OnClick, onClickBindMethod());

        public void AddContent(string content)
            => RendererTreeBuilder.AddContent(content);

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

        public void OpenElement(string elementName, string className)
        {
            OpenElement(elementName);
            AddCssClass(className);
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

        public void AddCreateItemComponent()
        {
            RendererTreeBuilder.OpenComponent(typeof(CreateItemForm<>).MakeGenericType(GridConfiguration.CreateItemOptions.ItemType));
            RendererTreeBuilder.CloseComponent();
        }

        public object GetActualItemColumnValue(string columnName)
            => PropertyValueAccessor.GetValue(ActualItem, columnName);

        private void AddEventAttributes()
        {
            if (TableDataSet.GridViewEvents.SaveOperationFinished != null)
            {
                RendererTreeBuilder.AddAttribute(
                    nameof(ITableDataSet.GridViewEvents.SaveOperationFinished),
                    RuntimeHelpers.TypeCheck(TableDataSet.GridViewEvents.SaveOperationFinished));
            }
        }
    }
}
