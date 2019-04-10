using Blazor.FlexGrid.DataSet.Options;
using Microsoft.AspNetCore.Http.Extensions;
using System;

namespace Blazor.FlexGrid.DataSet.Http
{
    public class RequestOptions
    {
        public ILazyLoadingOptions LazyLoadingOptions { get; }

        public IPagingOptions PageableOptions { get; }

        public ISortingOptions SortingOptions { get; }

        public IGroupingOptions GroupingOptions { get; }

        public RequestOptions(
            ILazyLoadingOptions lazyLoadingOptions,
            IPagingOptions pageableOptions,
            ISortingOptions sortingOptions,
            IGroupingOptions groupingOptions)
        {
            this.LazyLoadingOptions = lazyLoadingOptions ?? throw new ArgumentNullException(nameof(lazyLoadingOptions));
            this.PageableOptions = pageableOptions ?? throw new ArgumentNullException(nameof(pageableOptions));
            this.SortingOptions = sortingOptions ?? throw new ArgumentNullException(nameof(sortingOptions));
            this.GroupingOptions = groupingOptions ?? throw new ArgumentNullException(nameof(groupingOptions));
        }

        public string BuildUrl()
        {
            if (string.IsNullOrWhiteSpace(LazyLoadingOptions.DataUri))
            {
                throw new ArgumentNullException($"When you using LazyLoadedTableDataAdapter you must specify " +
                    $"{nameof(Options.LazyLoadingOptions.DataUri)} for lazy data retrieving. If you do not want use lazy loading feature use CollectionTableDataAdapter instead.");
            }

            var query = new QueryBuilder(LazyLoadingOptions.RequestParams);
            PagingParams(query, PageableOptions);
            SortingParams(query, SortingOptions);
            GroupingParams(query, GroupingOptions);

            return $"{LazyLoadingOptions.DataUri}{query.ToString()}";
        }

        private void GroupingParams(QueryBuilder builder, IGroupingOptions groupingOptions)
        {
            if (!string.IsNullOrWhiteSpace(groupingOptions.GroupedProperty?.Name))
            {
                builder.Add("groupExpression", groupingOptions.GroupedProperty.Name);
            }
        }

        private void PagingParams(QueryBuilder builder, IPagingOptions pagingOptions)
        {
            builder.Add("pagenumber", pagingOptions.CurrentPage.ToString());
            builder.Add("pagesize", pagingOptions.PageSize.ToString());
        }

        private void SortingParams(QueryBuilder builder, ISortingOptions sortingOptions)
        {
            if (!string.IsNullOrWhiteSpace(sortingOptions.SortExpression))
            {
                builder.Add("sortExpression", sortingOptions.SortExpression);
                builder.Add("sortDescending", sortingOptions.SortDescending.ToString());
            }
        }
    }
}
