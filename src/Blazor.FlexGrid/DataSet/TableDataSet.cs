using Blazor.FlexGrid.Components.Configuration.ValueFormatters;
using Blazor.FlexGrid.Components.Events;
using Blazor.FlexGrid.Components.Renderers;
using Blazor.FlexGrid.DataSet.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Blazor.FlexGrid.DataSet
{
    public class TableDataSet<TItem> : ITableDataSet, IBaseTableDataSet<TItem> where TItem : class
    {
        private IQueryable<TItem> source;
        private HashSet<object> selectedItems;
        private HashSet<object> deletedItems;

        public IPagingOptions PageableOptions { get; set; } = new PageableOptions();

        public ISortingOptions SortingOptions { get; set; } = new SortingOptions();

        public IRowEditOptions RowEditOptions { get; set; } = new RowEditOptions();

        public IGroupingOptions GroupingOptions { get; set; } = new GroupingOptions();

        public GridViewEvents GridViewEvents { get; set; } = new GridViewEvents();
        /// <summary>
        /// Gets or sets the items for the current page.
        /// </summary>
        public IList<TItem> Items { get; set; } = new List<TItem>();

        IList IBaseTableDataSet.Items => Items is List<TItem> list ? list : Items.ToList();

        public Action<TItem> OnItemClicked { get; set; }

        public Func<GridRendererContext, Action> OnRowClicked
        {
            get
            {
                return (context) =>
                {

                    TItem actualItem = Items.SingleOrDefault(it => (TItem)context.ActualItem == it);
                    if (OnItemClicked != null && actualItem != null)
                        return () => OnItemClicked(actualItem);
                    else
                        return () => { };


                };
            }
        }

        public IEnumerable GroupedItems { get; set; }

        public TableDataSet(IQueryable<TItem> source)
        {
            this.source = source ?? throw new ArgumentNullException(nameof(source));
            this.selectedItems = new HashSet<object>();
            this.deletedItems = new HashSet<object>();
        }

        public Task GoToPage(int index)
        {
            PageableOptions.CurrentPage = index;
            LoadFromQueryableSource();

            return Task.FromResult(0);
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

        private void LoadFromQueryableSource()
        {
            PageableOptions.TotalItemsCount = ApplyDeletedConditionToQueryable(source).Count();
            Items = ApplyFiltersToQueryable(source).ToList();
            if (GroupingOptions.IsGroupingActive)
                GroupedItems = ApplyGroupingToQueryable(source);
        }

        private IEnumerable ApplyGroupingToQueryable(IQueryable<TItem> source)
        {
            var groupedItems = this.GroupItems<TItem>(source);
            groupedItems = ApplyPagingToGroupedQueryable(groupedItems);
            groupedItems = ApplySortingToGroupedQueryable(groupedItems);
            return groupedItems;
        }

        private IEnumerable<IGrouping<object, TItem>> ApplySortingToGroupedQueryable(IEnumerable<IGrouping<object, TItem>> queryable)
        {
            if (!string.IsNullOrEmpty(SortingOptions?.SortExpression))
            {
                var sortedQueryable = queryable;
                foreach (var group in sortedQueryable)
                {
                    ApplySortingToQueryable(group.AsQueryable());
                }
                return sortedQueryable;
            }
            else
            {
                return queryable;
            }
        }

        private IEnumerable<IGrouping<object, TItem>> ApplyPagingToGroupedQueryable(IEnumerable<IGrouping<object, TItem>> queryable)
        {
            PageableOptions.TotalItemsCount = queryable.Count();
            return ApplyPagingToQueryable<IGrouping<object, TItem>>(queryable.AsQueryable());
        }

        private IQueryable<TItem> ApplyFiltersToQueryable(IQueryable<TItem> queryable)
        {
            queryable = ApplyDeletedConditionToQueryable(queryable);
            queryable = ApplySortingToQueryable(queryable);
            queryable = ApplyPagingToQueryable(queryable);

            return queryable;
        }

        private IQueryable<TItem> ApplyPagingToQueryable(IQueryable<TItem> queryable)
        {

            return ApplyPagingToQueryable<TItem>(queryable);

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
                ? source.OrderByDescending(SortingOptions.SortExpression)
                : source.OrderBy(SortingOptions.SortExpression);
        }





    }
}
