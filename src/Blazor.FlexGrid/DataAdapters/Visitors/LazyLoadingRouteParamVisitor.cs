using Blazor.FlexGrid.Components.Configuration.MetaData;
using Blazor.FlexGrid.Components.Configuration.ValueFormatters;
using Blazor.FlexGrid.DataSet.Options;
using System;

namespace Blazor.FlexGrid.DataAdapters.Visitors
{
    public class LazyLoadingRouteParamVisitor : IDataTableAdapterVisitor
    {
        private readonly IMasterDetailRelationship masterDetailRelationship;
        private readonly IMasterDetailRowArguments masterDetailRowArguments;
        private readonly ITypePropertyAccessorCache propertyValueAccessorCache;

        public LazyLoadingRouteParamVisitor(
            IMasterDetailRelationship masterDetailRelationship,
            IMasterDetailRowArguments masterDetailRowArguments,
            ITypePropertyAccessorCache propertyValueAccessorCache
            )
        {
            this.masterDetailRelationship = masterDetailRelationship ?? throw new ArgumentNullException(nameof(masterDetailRelationship));
            this.masterDetailRowArguments = masterDetailRowArguments ?? throw new ArgumentNullException(nameof(masterDetailRowArguments));
            this.propertyValueAccessorCache = propertyValueAccessorCache ?? throw new ArgumentNullException(nameof(propertyValueAccessorCache));
        }

        public void Visit(ITableDataAdapter tableDataAdapter)
        {
            if (tableDataAdapter is ILazyLoadedTableDataAdapter lazyLoadedTableDataAdapter)
            {
                var selectedItemType = masterDetailRowArguments.SelectedItem.GetType();
                var detailAdapterItemType = masterDetailRowArguments.DataAdapter.UnderlyingTypeOfItem;

                var constantValue = propertyValueAccessorCache
                    .GetPropertyAccesor(selectedItemType)
                    .GetValue(masterDetailRowArguments.SelectedItem, masterDetailRelationship.MasterDetailConnection.MasterPropertyName);

                lazyLoadedTableDataAdapter.AddRequestParamsAction = reqParams => reqParams
                    .Add(masterDetailRelationship.MasterDetailConnection.ForeignPropertyName, constantValue.ToString());
            }
        }
    }
}
