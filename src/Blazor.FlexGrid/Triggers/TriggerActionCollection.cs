using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.FlexGrid.Triggers
{
    public class TriggerActionCollection : IParameterActionTriggerCollection
    {
        private readonly List<IParamterChangedTrigger> paramterChangedTriggers;

        public bool HasMasterAction => paramterChangedTriggers.Any(t => t.IsMasterAction);

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

            if (paramterChangedTriggers.Any(t => t.IsMasterAction))
            {
                return false;
            }

            paramterChangedTriggers.Add(trigger);

            return true;
        }

        public Task ExecuteTriggers()
        {
            if (!paramterChangedTriggers.Any())
            {
                return Task.FromResult(0);
            }

            foreach (var trigger in paramterChangedTriggers)
            {
                trigger.Execute();
            }

            paramterChangedTriggers.Clear();
            return Task.FromResult(0);
        }

        public async Task ExecuteTriggers(Func<Task> onActionExecuted)
        {
            await ExecuteTriggers();
            await onActionExecuted();
        }
    }
}
