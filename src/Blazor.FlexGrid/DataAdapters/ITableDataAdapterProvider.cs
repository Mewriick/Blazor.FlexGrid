using Blazor.FlexGrid.DataSet;
using Blazor.FlexGrid.Features;
using System;
using System.Reflection;

namespace Blazor.FlexGrid.DataAdapters
{
    public interface ITableDataAdapterProvider
    {
        ITableDataAdapter ConvertToDetailTableDataAdapter(ITableDataAdapter tableDataAdapter, object selectedItem);

        ITableDataAdapter CreateCollectionTableDataAdapter(object selectedItem, PropertyInfo propertyInfo);

        ITableDataAdapter CreateCollectionTableDataAdapter(Type dataSetType, GroupItem group);

        ITableDataAdapter CreateMasterTableDataAdapter(ITableDataAdapter mainTableDataAdapter, IMasterTableFeature masterTableFeature);
    }
}
