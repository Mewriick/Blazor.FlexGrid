using System.Reflection;

namespace Blazor.FlexGrid.DataAdapters
{
    public interface ITableDataAdapterProvider
    {
        ITableDataAdapter ConvertToDetailTableDataAdapter(ITableDataAdapter tableDataAdapter, object selectedItem);

        ITableDataAdapter CreateCollectionTableDataAdapter(object selectedItem, PropertyInfo propertyInfo);
    }
}
