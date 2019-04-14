using Blazor.FlexGrid.Components.Configuration.ValueFormatters;
using Blazor.FlexGrid.Components.Events;
using Blazor.FlexGrid.DataSet.Options;
using Blazor.FlexGrid.Filters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Blazor.FlexGrid.DataSet
{
    public class TableDataSet<TItem> : ITableDataSet, IBaseTableDataSet<TItem> where TItem : class
    {
        private readonly IFilterExpressionTreeBuilder<TItem> filterExpressionTreeBuilder;

        private IQueryable<TItem> source;
        private Expression<Func<TItem, bool>> filterExpression;
        private HashSet<object> selectedItems;
        private HashSet<object> deletedItems;

        public IPagingOptions PageableOptions { get; set; } = new PageableOptions();

        public ISortingOptions SortingOptions { get; set; } = new SortingOptions();

        public IRowEditOptions RowEditOptions { get; set; } = new RowEditOptions();

        public IGroupingOptions GroupingOptions { get; set; } = new GroupingOptions();

        public GridViewEvents GridViewEvents { get; set; } = new GridViewEvents();

        public bool FilterIsApplied => filterExpression != null;

        /// <summary>
        /// Gets or sets the items for the current page.
        /// </summary>
        public IList<TItem> Items { get; set; } = new List<TItem>();

        IList IBaseTableDataSet.Items => Items is List<TItem> list ? list : Items.ToList();

        public IList<GroupItem> GroupedItems { get; private set; }

        public TableDataSet(
            IQueryable<TItem> source,
            IFilterExpressionTreeBuilder<TItem> filterExpressionTreeBuilder)
        {
            this.source = source ?? throw new ArgumentNullException(nameof(source));
            this.filterExpressionTreeBuilder = filterExpressionTreeBuilder ?? throw new ArgumentNullException(nameof(filterExpressionTreeBuilder));
            this.selectedItems = new HashSet<object>();
            this.deletedItems = new HashSet<object>();
        }

        public Task GoToPage(int index)
        {
            PageableOptions.CurrentPage = index;
            ApplyFiltersToQueryableSource(source);

            return Task.CompletedTask;
        }

        public Task SetSortExpression(string expression)
        {
            if (SortingOptions.SortExpression != expression)
            {
                SortingOptions.SortExpression = expression;
                SortingOptions.SortDescending = false;
            }
            else
            {
                SortingOptions.SortDescending = !SortingOptions.SortDescending;
            }

            return GoToPage(0);
        }

        public Task ApplyFilters(IReadOnlyCollection<IFilterDefinition> filters)
        {
            if (!filters.Any())
            {
                filterExpression = null;
                return GoToPage(0);
            }

            PageableOptions.CurrentPage = 0;
            filterExpression = filterExpressionTreeBuilder.BuildExpressionTree(filters);

            return GoToPage(0);
        }

        public void ToggleRowItem(object item)
        {
            if (ItemIsSelected(item))
            {
                selectedItems.Remove(item);
                return;
            }

            selectedItems.Add(item);
        }

        public bool ItemIsSelected(object item)
            => selectedItems.Contains(item);

        public void StartEditItem(object item)
        {
            if (item != null)
            {
                RowEditOptions.ItemInEditMode = item;
            }
        }

        public void EditItemProperty(string propertyName, object propertyValue)
            => RowEditOptions.AddNewValue(propertyName, propertyValue);

        public Task<bool> SaveItem(ITypePropertyAccessor propertyValueAccessor)
        {
            try
            {
                foreach (var newValue in RowEditOptions.UpdatedValues)
                {
                    propertyValueAccessor.SetValue(RowEditOptions.ItemInEditMode, newValue.Key, newValue.Value);
                }

                GridViewEvents.SaveOperationFinished?.Invoke(new SaveResultArgs { ItemSucessfullySaved = true, Item = RowEditOptions.ItemInEditMode });

                return Task.FromResult(true);
            }
            catch (Exception)
            {
                GridViewEvents.SaveOperationFinished?.Invoke(new SaveResultArgs { ItemSucessfullySaved = false });
                return Task.FromResult(false);
            }
            finally
            {
                RowEditOptions.ItemInEditMode = EmptyDataSetItem.Instance;
            }
        }

        public async Task<bool> DeleteItem(object item)
        {
            var removeResult = Items.Remove((TItem)item);
            if (removeResult)
            {
                PageableOptions.TotalItemsCount--;
                deletedItems.Add(item);
                GridViewEvents.DeleteOperationFinished?.Invoke(new DeleteResultArgs { ItemSuccesfullyDeleted = true, Item = item });
                await GoToPage(PageableOptions.CurrentPage);
            }

            GridViewEvents.DeleteOperationFinished?.Invoke(new DeleteResultArgs { ItemSuccesfullyDeleted = false, Item = item });

            return removeResult;
        }

        public void CancelEditation()
            => RowEditOptions.ItemInEditMode = EmptyDataSetItem.Instance;

        public void ToggleGroupRow(object groupItemKey)
            => GroupedItems.ToggleGroup(groupItemKey);

        private void ApplyFiltersToQueryableSource(IQueryable<TItem> source)
        {
            var filteredSource = filterExpression is null ? source : source.Where(filterExpression);
            var sourceWithoutDeleted = ApplyDeletedConditionToQueryable(filteredSource);
            if (!GroupingOptions.IsGroupingActive)
            {
                PageableOptions.TotalItemsCount = sourceWithoutDeleted.Count();
                Items = ApplyFiltersToQueryable(sourceWithoutDeleted).ToList();
            }
            else
            {
                var newGroupedItems = ApplyFiltersWithGroupingToQueryable(sourceWithoutDeleted).OfType<GroupItem>().ToList();
                newGroupedItems.PreserveGroupCollapsing(GroupedItems);
                GroupedItems = newGroupedItems;
            }
        }

        private IQueryable<GroupItem<TItem>> ApplyFiltersWithGroupingToQueryable(IQueryable<TItem> source)
        {
            var groupedItemsQueryable = ApplyGroupingToQueryable(source);
            groupedItemsQueryable = ApplySortingToGroupedQueryable(groupedItemsQueryable);
            PageableOptions.TotalItemsCount = groupedItemsQueryable.Count();
            groupedItemsQueryable = ApplyPagingToQueryable(groupedItemsQueryable);

            return groupedItemsQueryable;
        }

        private IQueryable<GroupItem<TItem>> ApplySortingToGroupedQueryable(IQueryable<GroupItem<TItem>> queryable)
        {
            if (string.IsNullOrEmpty(SortingOptions?.SortExpression))
            {
                return queryable;
            }

            if (SortingOptions.SortExpression != GroupingOptions.GroupedProperty.Name)
            {
                queryable = queryable.Select(x => new GroupItem<TItem>(x.Key,
                                SortingOptions.SortDescending
                                    ? x.AsQueryable().OrderByDescending(SortingOptions.SortExpression)
                                    : x.AsQueryable().OrderBy(SortingOptions.SortExpression)));
                return queryable;
            }
            else
                return SortingOptions.SortDescending
                    ? queryable.OrderByDescending(x => x.Key)
                    : queryable.OrderBy(x => x.Key);
        }

        private IQueryable<GroupItem<TItem>> ApplyGroupingToQueryable(IQueryable<TItem> source)
        {
            return source.GroupBy(GroupingOptions.GroupedProperty.Name, "it")
                .Select<GroupItem<TItem>>(ParsingConfig.Default, "new (it.Key as Key, it as Items)");
        }

        private IQueryable<TItem> ApplyFiltersToQueryable(IQueryable<TItem> queryable)
        {
            queryable = ApplySortingToQueryable(queryable);
            queryable = ApplyPagingToQueryable(queryable);

            return queryable;
        }

        private IQueryable<T> ApplyPagingToQueryable<T>(IQueryable<T> queryable)
        {
            return PageableOptions != null && PageableOptions.PageSize > 0
                ? queryable.Skip(PageableOptions.PageSize * PageableOptions.CurrentPage)
                    .Take(PageableOptions.PageSize)
                : queryable;
        }

        private IQueryable<TItem> ApplyDeletedConditionToQueryable(IQueryable<TItem> queryable)
            => !deletedItems.Any()
                ? queryable
                : queryable.Where(i => !deletedItems.Contains(i));


        private IQueryable<TItem> ApplySortingToQueryable(IQueryable<TItem> queryable)
        {
            if (string.IsNullOrEmpty(SortingOptions?.SortExpression))
            {
                return queryable;
            }

            return SortingOptions.SortDescending
                ? queryable.OrderByDescending(SortingOptions.SortExpression)
                : queryable.OrderBy(SortingOptions.SortExpression);
        }
    }
}
