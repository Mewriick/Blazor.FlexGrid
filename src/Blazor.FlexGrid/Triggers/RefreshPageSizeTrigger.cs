using Blazor.FlexGrid.DataSet.Options;
using System;
using System.Threading.Tasks;

namespace Blazor.FlexGrid.Triggers
{
    public class RefreshPageSizeTrigger : IParamterChangedTrigger
    {
        private readonly IPagingOptions pagingOptions;
        private readonly int newPageSize;

        public bool RefreshPage => false;

        public RefreshPageSizeTrigger(IPagingOptions pagingOptions, int newPageSize)
        {
            this.pagingOptions = pagingOptions ?? throw new ArgumentNullException(nameof(pagingOptions));
            this.newPageSize = newPageSize;
        }

        public Task Execute()
        {
            var lastCurrentPage = pagingOptions.CurrentPage;
            pagingOptions.PageSize = newPageSize;

            if (lastCurrentPage > pagingOptions.PagesCount)
            {
                pagingOptions.CurrentPage = pagingOptions.PagesCount - 1;
            }

            return Task.CompletedTask;
        }
    }
}
