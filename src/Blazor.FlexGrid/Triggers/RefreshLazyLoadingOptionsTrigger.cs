using Blazor.FlexGrid.DataSet;
using Blazor.FlexGrid.DataSet.Options;
using System;
using System.Threading.Tasks;

namespace Blazor.FlexGrid.Triggers
{
    class RefreshLazyLoadingOptionsTrigger : IParamterChangedTrigger
    {
        private readonly ITableDataSet tableDataSet;
        private readonly ILazyLoadingOptions newLazyLoadingOptions;

        public bool RefreshPage => false;

        public RefreshLazyLoadingOptionsTrigger(ITableDataSet tableDataSet, ILazyLoadingOptions newLazyLoadingOptions)
        {
            this.tableDataSet = tableDataSet ?? throw new ArgumentNullException(nameof(tableDataSet));
            this.newLazyLoadingOptions = newLazyLoadingOptions ?? throw new ArgumentNullException(nameof(newLazyLoadingOptions));
        }

        public Task Execute()
        {
            if (tableDataSet is ILazyTableDataSet lazyTableDataSet)
            {
                lazyTableDataSet.LazyLoadingOptions.Copy(newLazyLoadingOptions);
            }

            return Task.CompletedTask;
        }
    }
}
