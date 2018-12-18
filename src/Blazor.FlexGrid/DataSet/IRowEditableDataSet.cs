using Blazor.FlexGrid.Components.Configuration.ValueFormatters;
using Blazor.FlexGrid.DataSet.Options;
using System.Threading.Tasks;

namespace Blazor.FlexGrid.DataSet
{
    public interface IRowEditableDataSet : IBaseTableDataSet
    {
        IRowEditOptions RowEditOptions { get; }

        void StartEditItem(object item);

        void EditItemProperty(string propertyName, object propertyValue);

        void CancelEditation();

        Task<bool> SaveItem(IPropertyValueAccessor propertyValueAccessor);
    }
}
