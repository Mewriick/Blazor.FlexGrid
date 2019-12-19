using System;

namespace Blazor.FlexGrid.Triggers
{
    public class RefreshLazyDataSetTrigger : IParamterChangedTrigger
    {
        public bool IsMasterAction => false;

        public void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
