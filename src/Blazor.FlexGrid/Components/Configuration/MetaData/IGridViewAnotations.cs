using System;

namespace Blazor.FlexGrid.Components.Configuration.MetaData
{
    public interface IGridViewAnotations
    {
        bool IsMasterTable { get; }

        bool InlineEditIsAllowed { get; }

        GridCssClasses CssClasses { get; }

        IMasterDetailRelationship FindRelationshipConfiguration(Type detailType);
    }
}
