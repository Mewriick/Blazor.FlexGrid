using Blazor.FlexGrid.Components.Configuration.MetaData;
using Blazor.FlexGrid.Components.Configuration.ValueFormatters;
using Blazor.FlexGrid.DataSet.Options;
using System;
using System.Linq.Expressions;

namespace Blazor.FlexGrid.DataAdapters.Visitors
{
    public class FilterVisitor<TItem> : IDataTableAdapterVisitor where TItem : class
    {
        private readonly IMasterDetailRelationship masterDetailRelationship;
        private readonly IMasterDetailRowArguments masterDetailRowArguments;
        private readonly ITypePropertyAccessorCache propertyValueAccessorCache;

        public FilterVisitor(
            IMasterDetailRelationship masterDetailRelationship,
            IMasterDetailRowArguments masterDetailRowArguments,
            ITypePropertyAccessorCache propertyValueAccessorCache)
        {
            this.masterDetailRelationship = masterDetailRelationship ?? throw new ArgumentNullException(nameof(masterDetailRelationship));
            this.masterDetailRowArguments = masterDetailRowArguments ?? throw new ArgumentNullException(nameof(masterDetailRowArguments));
            this.propertyValueAccessorCache = propertyValueAccessorCache ?? throw new ArgumentNullException(nameof(propertyValueAccessorCache));
        }

        public void Visit(ITableDataAdapter tableDataAdapter)
        {
            if (tableDataAdapter is CollectionTableDataAdapter<TItem> collectionTableDataAdapter)
            {
                var selectedItemType = masterDetailRowArguments.SelectedItem.GetType();
                var detailAdapterItemType = masterDetailRowArguments.DataAdapter.UnderlyingTypeOfItem;

                var constantValue = propertyValueAccessorCache
                    .GetPropertyAccesor(selectedItemType)
                    .GetValue(masterDetailRowArguments.SelectedItem, masterDetailRelationship.MasterDetailConnection.MasterPropertyName);

                var parameter = Expression.Parameter(detailAdapterItemType, "x");
                var member = Expression.Property(parameter, masterDetailRelationship.MasterDetailConnection.ForeignPropertyName);

                //If the type of the member expression is a nullable,
                //the call to Expression.Equal will fail
                Expression constant;
                if (member.Type.IsGenericType && member.Type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    constant = Expression.Constant(constantValue, member.Type);
                else
                    constant = Expression.Constant(constantValue);

                var body = Expression.Equal(member, constant);

                collectionTableDataAdapter.Filter = Expression.Lambda<Func<TItem, bool>>(body, parameter);
            }
        }
    }
}
