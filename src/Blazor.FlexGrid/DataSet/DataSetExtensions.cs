using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Blazor.FlexGrid.DataSet
{
    public static class DataSetExtensions
    {
        public static Type UnderlyingTypeOfItem(this ITableDataSet tableDataSet)
            => tableDataSet.GetType().GenericTypeArguments[0];

        public static bool HasItems(this ITableDataSet tableDataSet)
            => !(tableDataSet.Items == null || tableDataSet.Items.Count <= 0);

        public static Task GoToNextPage(this ITableDataSet tableDataSet)
        {
            if (!tableDataSet.PageableOptions.IsLastPage)
            {
                return tableDataSet.GoToPage(tableDataSet.PageableOptions.CurrentPage + 1);
            }

            return Task.FromResult(0);
        }

        public static Task GoToPreviousPage(this ITableDataSet tableDataSet)
        {
            if (!tableDataSet.PageableOptions.IsFirstPage)
            {
                return tableDataSet.GoToPage(tableDataSet.PageableOptions.CurrentPage - 1);
            }

            return Task.FromResult(0);
        }

        public static async Task GoToFirstPage(this ITableDataSet tableDataSet)
        {
            if (!tableDataSet.PageableOptions.IsFirstPage)
            {
                await tableDataSet.GoToPage(0);
            }
        }

        public static async Task GoToLastPage(this ITableDataSet tableDataSet)
        {
            if (!tableDataSet.PageableOptions.IsLastPage)
            {
                await tableDataSet.GoToPage(tableDataSet.PageableOptions.PagesCount - 1);
            }
        }

        public static string PageInfoText(this ITableDataSet tableDataSet)
        {
            var from = tableDataSet.PageableOptions.CurrentPage * tableDataSet.PageableOptions.PageSize + 1;
            var to = from + tableDataSet.PageableOptions.PageSize - 1;

            return $"{from} - {Math.Min(to, tableDataSet.PageableOptions.TotalItemsCount)}";
        }

        public static bool IsItemEdited(this ITableDataSet tableDataSet, object item)
            => tableDataSet.RowEditOptions.ItemInEditMode == item;



        public static IQueryable<GroupItem<TItem>> GroupItems<TItem>(
            this IGroupableTableDataSet tableDataSet, IQueryable<TItem> source)
        {
            var groupingOptions = tableDataSet.GroupingOptions;
            if (groupingOptions.IsGroupingActive)
            {
                try
                {

                    var param = Expression.Parameter(typeof(TItem));
                    var propertyOrField = Expression.PropertyOrField(param, groupingOptions.GroupedProperty.Name);

                    var callToString = Expression.Call(propertyOrField,
                        propertyOrField.Type.GetMethods().FirstOrDefault(m => m.Name == "ToString"));

                    var callExprReturnLabel = Expression.Label(typeof(string));

                    Expression<Func<TItem, string>> keyExpression;

                    if (CanAcceptNullValueExpression(propertyOrField.Type))
                    {
                        var test = Expression.Equal(propertyOrField, Expression.Constant(null, propertyOrField.Type));
                        var returnIfTrue = Expression.Return(callExprReturnLabel, Expression.Constant(null, typeof(string)));
                        var returnIfFalse = Expression.Return(callExprReturnLabel, callToString);
                        var callAndNullCheckExpr = Expression.IfThenElse(
                            test, returnIfTrue, returnIfFalse);


                        var lambdaBodyBlock = Expression.Block(callAndNullCheckExpr,
                            Expression.Label(callExprReturnLabel, Expression.Constant(null, typeof(string))));
                        keyExpression = Expression.Lambda<Func<TItem, string>>(lambdaBodyBlock, param);
                    }
                    else
                    {
                        keyExpression = Expression.Lambda<Func<TItem, string>>(callToString, param);
                    }

                    return source.GroupBy(keyExpression)
                                .Select(grp => new GroupItem<TItem>(grp.Key, grp));


                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                return null;
            }

        }

        private static bool CanAcceptNullValueExpression(Type type)
        {
            try
            {
                Expression.Constant(null, type);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static IQueryable<GroupItem<TItem>> RetrieveGroupItemsIfCollapsedValues<TItem>(this IQueryable<GroupItem<TItem>> newGroupedItems,
                                                        IEnumerable<GroupItem> oldGroupedItems)
        {
            if (oldGroupedItems == null)
            {
                return newGroupedItems;
            }
            else
            {

                //Join query with pre-existing GroupItems collection in order not to lose "IsCollapsed" values
                string defaultStrValueIfNull = Guid.NewGuid().ToString();
                var queryIfCollapsedValues = from newItem in newGroupedItems
                                             join oldItem in oldGroupedItems on newItem.Key ?? defaultStrValueIfNull
                                                                           equals oldItem.Key ?? defaultStrValueIfNull
                                                                           into joinQuery
                                             from x in joinQuery.DefaultIfEmpty()
                                             select new GroupItem<TItem>(newItem.Key, newItem.Items) { IsCollapsed = x != null ? x.IsCollapsed : true };

                return queryIfCollapsedValues;
            }
        }

        public static void ToggleGroupRow<TItem>(this ITableDataSet tableDataSet, object groupItemKey)
        {
            var keyEqualityComparer = new GroupingKeyEqualityComparer();
            var groupItemToToggle = tableDataSet.GroupedItems.FirstOrDefault(item => keyEqualityComparer.Equals(item.Key, groupItemKey));
            groupItemToToggle.IsCollapsed = !groupItemToToggle.IsCollapsed;
        }
    }
}