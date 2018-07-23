using System;

namespace Blazor.FlexGrid.Components.Configuration.MetaData
{
    public interface IGridViewAnotations
    {
        bool IsMasterTable { get; }

        IMasterDetailRelationship FindRelationshipConfiguration(Type detailType);
    }
}
