using System;

namespace Blazor.FlexGrid.Components
{
    public class EditColumnContext
    {
        private readonly string columnName;
        private readonly Action<string, object> onChangeAction;

        public EditColumnContext(string columnName, Action<string, object> onChangeAction)
        {
            this.columnName = string.IsNullOrWhiteSpace(columnName)
                ? throw new ArgumentNullException(nameof(columnName))
                : columnName;

            this.onChangeAction = onChangeAction ?? throw new ArgumentNullException(nameof(onChangeAction));
        }

        public void NotifyValueHasChanged(object value)
        {
            onChangeAction.Invoke(columnName, value);
        }
    }
}
