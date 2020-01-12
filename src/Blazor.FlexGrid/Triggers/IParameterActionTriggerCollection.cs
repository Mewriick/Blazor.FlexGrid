using System;
using System.Threading.Tasks;

namespace Blazor.FlexGrid.Triggers
{
    public interface IParameterActionTriggerCollection : ITriggerCollection
    {
        bool AddTrigger(IParamterChangedTrigger trigger);

        Task ExecuteTriggers(Func<Task> onActionExecuted);
    }
}
