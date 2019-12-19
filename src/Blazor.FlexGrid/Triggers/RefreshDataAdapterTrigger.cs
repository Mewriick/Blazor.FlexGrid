using Blazor.FlexGrid.DataSet;
using System;

namespace Blazor.FlexGrid.Triggers
{
    public class RefreshDataAdapterTrigger : IParamterChangedTrigger
    {
        private readonly Func<ITableDataSet> createDataSet;

        public bool IsMasterAction => true;

        public RefreshDataAdapterTrigger(Func<ITableDataSet> createDataSet)
        {
            this.createDataSet = createDataSet ?? throw new ArgumentNullException(nameof(createDataSet));
        }

        public void Execute()
        {
            createDataSet();
        }
    }
}
