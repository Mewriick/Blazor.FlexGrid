using Blazor.FlexGrid.DataSet;
using System;
using System.Threading.Tasks;

namespace Blazor.FlexGrid.Triggers
{
    public class RefreshDataAdapterTrigger : IParamterChangedTrigger
    {
        private readonly Func<ITableDataSet> createDataSet;

        public bool RefreshPage => true;

        public RefreshDataAdapterTrigger(Func<ITableDataSet> createDataSet)
        {
            this.createDataSet = createDataSet ?? throw new ArgumentNullException(nameof(createDataSet));
        }

        public async Task Execute()
        {
            var tableDataSet = createDataSet();
            await tableDataSet.GoToPage(0);
        }
    }
}
