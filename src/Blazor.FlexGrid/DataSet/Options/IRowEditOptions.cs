using System.Collections.Generic;

namespace Blazor.FlexGrid.DataSet.Options
{
    public interface IRowEditOptions
    {
        object ItemInEditMode { get; set; }

        IReadOnlyDictionary<string, object> UpdatedValues { get; }

        void AddNewValue(string propertyName, object value);
    }
}
