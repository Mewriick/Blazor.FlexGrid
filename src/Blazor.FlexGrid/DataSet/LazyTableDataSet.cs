using Blazor.FlexGrid.Components.Configuration.ValueFormatters;
using Blazor.FlexGrid.Components.Events;
using Blazor.FlexGrid.DataSet.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.FlexGrid.DataSet
{
    /// <summary>
    /// Collection of items which are fetched from server API
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public class LazyTableDataSet<TItem> : ILazyTableDataSet, IBaseTableDataSet<TItem> where TItem : class
    {
        private readonly ILazyDataSetLoader<TItem> lazyDataSetLoader;
        private readonly ILazyDataSetItemManipulator<TItem> lazyDataSetItemSaver;
        private HashSet<object> selectedItems;

        public IPagingOptions PageableOptions { get; set; } = new PageableOptions();

        public ISortingOptions SortingOptions { get; set; } = new SortingOptions();

        public ILazyLoadingOptions LazyLoadingOptions { get; set; } = new LazyLoadingOptions();

        public IRowEditOptions RowEditOptions { get; set; } = new RowEditOptions();

        public GridViewEvents GridViewEvents { get; set; } = new GridViewEvents();

        /// <summary>
        /// Gets or sets the items for the current page.
        /// </summary>
        public IList<TItem> Items { get; set; } = new List<TItem>();

        IList IBaseTableDataSet.Items => Items is List<TItem> list ? list : Items.ToList();



        public LazyTableDataSet(ILazyDataSetLoader<TItem> lazyDataSetLoader, ILazyDataSetItemManipulator<TItem> lazyDataSetItemSaver)
        {
            this.lazyDataSetLoader = lazyDataSetLoader ?? throw new ArgumentNullException(nameof(lazyDataSetLoader));
            this.lazyDataSetItemSaver = lazyDataSetItemSaver ?? throw new ArgumentNullException(nameof(lazyDataSetItemSaver));
            this.selectedItems = new HashSet<object>();
        }

        public async Task GoToPage(int index)
        {
            PageableOptions.CurrentPage = index;
            var pagedDataResult = await lazyDataSetLoader.GetTablePageData(LazyLoadingOptions, PageableOptions, SortingOptions);
            PageableOptions.TotalItemsCount = pagedDataResult.TotalCount;
            Items = pagedDataResult.Items;
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

        public async Task<bool> SaveItem(ITypePropertyAccessor propertyValueAccessor)
        {
            try
            {
                foreach (var newValue in RowEditOptions.UpdatedValues)
                {
                    propertyValueAccessor.SetValue(RowEditOptions.ItemInEditMode, newValue.Key, newValue.Value);
                }
            }
            catch (Exception)
            {
                GridViewEvents.SaveOperationFinished?.Invoke(new SaveResultArgs { ItemSucessfullySaved = false });
                RowEditOptions.ItemInEditMode = EmptyDataSetItem.Instance;

                return false;
            }

            var typedItem = (TItem)RowEditOptions.ItemInEditMode;
            var saveResult = await lazyDataSetItemSaver.SaveItem(typedItem, LazyLoadingOptions);
            if (saveResult != null)
            {
                var itemIndex = Items.IndexOf(typedItem);
                if (itemIndex > -1)
                {
                    Items[itemIndex] = saveResult;
                }

                GridViewEvents.SaveOperationFinished?.Invoke(new SaveResultArgs { ItemSucessfullySaved = true, Item = saveResult });
            }

            RowEditOptions.ItemInEditMode = EmptyDataSetItem.Instance;

            return saveResult != null ? true : false;
        }

        public void CancelEditation()
            => RowEditOptions.ItemInEditMode = EmptyDataSetItem.Instance;

        public async Task<bool> DeleteItem(object item)
        {
            var typedItem = (TItem)item;
            var removedItem = await lazyDataSetItemSaver.DeleteItem(typedItem, LazyLoadingOptions);
            if (removedItem != null)
            {
                GridViewEvents.DeleteOperationFinished?.Invoke(new DeleteResultArgs { ItemSuccesfullyDeleted = true, Item = removedItem ?? item });
                await GoToPage(PageableOptions.CurrentPage);
            }
            else
            {
                GridViewEvents.DeleteOperationFinished?.Invoke(new DeleteResultArgs { ItemSuccesfullyDeleted = false, Item = item });
            }

            return removedItem != null ? true : false;
        }
    }
}
