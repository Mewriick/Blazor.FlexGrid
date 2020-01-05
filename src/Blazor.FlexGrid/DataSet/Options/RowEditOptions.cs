using System.Collections.Generic;

namespace Blazor.FlexGrid.DataSet.Options
{
    public class RowEditOptions : IRowEditOptions
    {
        private Dictionary<string, object> updatedValues;
        private object itemInEditMode;

        public object ItemInEditMode
        {
            get
            {
                return itemInEditMode;
            }
            set
            {
                itemInEditMode = value;
                if (itemInEditMode is EmptyDataSetItem)
                {
                    updatedValues.Clear();
                }
            }
        }

        public IReadOnlyDictionary<string, object> UpdatedValues => updatedValues;

        public RowEditOptions()
        {
            this.updatedValues = new Dictionary<string, object>();
            ItemInEditMode = EmptyDataSetItem.Instance;
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
