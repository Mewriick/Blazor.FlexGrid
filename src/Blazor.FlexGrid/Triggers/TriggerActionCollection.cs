using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.FlexGrid.Triggers
{
    public class TriggerActionCollection : IParameterActionTriggerCollection
    {
        private readonly List<IParamterChangedTrigger> paramterChangedTriggers;

        private bool HasActionWithRefresh => paramterChangedTriggers.Any(t => t.RefreshPage);

        public TriggerActionCollection()
        {
            this.paramterChangedTriggers = new List<IParamterChangedTrigger>();
        }

        public bool AddTrigger(IParamterChangedTrigger trigger)
        {
            if (trigger is null)
            {
                throw new ArgumentNullException(nameof(trigger));
            }

            if (paramterChangedTriggers.Any(t => t.RefreshPage))
            {
                return false;
            }

            paramterChangedTriggers.Add(trigger);

            return true;
        }

        public async Task ExecuteTriggers()
        {
            await Task.WhenAll(paramterChangedTriggers.Select(t => t.Execute()));

            return;
        }

        public async Task ExecuteTriggers(Func<Task> onActionExecuted)
        {
            if (!paramterChangedTriggers.Any())
            {
                return;
            }

            await ExecuteTriggers();

            if (!HasActionWithRefresh)
            {
                Console.WriteLine("ExecuteTriggers Refresh");
                await onActionExecuted();
            }

            paramterChangedTriggers.Clear();
        }
    }
}
