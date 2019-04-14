using Blazor.FlexGrid.DataSet;
using System;
using System.Reflection;

namespace Blazor.FlexGrid.DataAdapters
{
    public interface ITableDataAdapterProvider
    {
        ITableDataAdapter ConvertToDetailTableDataAdapter(ITableDataAdapter tableDataAdapter, object selectedItem);

        ITableDataAdapter CreateCollectionTableDataAdapter(object selectedItem, PropertyInfo propertyInfo);

        ITableDataAdapter CreateCollectionTableDataAdapter(Type dataSetType, GroupItem group);
    }
}
