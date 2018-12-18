using System.Collections.Generic;

namespace Blazor.FlexGrid.DataSet.Options
{
    public class RowEditOptions : IRowEditOptions
    {
        private Dictionary<string, object> updatedValues;

        public object ItemInEditMode { get; set; } = EmptyDataSetItem.Instance;

        public IReadOnlyDictionary<string, object> UpdatedValues => updatedValues;

        public RowEditOptions()
        {
            this.updatedValues = new Dictionary<string, object>();
        }

        public void AddNewValue(string propertyName, object value)
        {
            if (updatedValues.ContainsKey(propertyName))
            {
                updatedValues[propertyName] = value;
            }
            else
            {
                updatedValues.Add(propertyName, value);
            }
        }
    }
}
