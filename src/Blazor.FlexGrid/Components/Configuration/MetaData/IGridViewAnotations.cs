using System;

namespace Blazor.FlexGrid.Components.Configuration.MetaData
{
    public interface IGridViewAnotations
    {
        bool IsMasterTable { get; }

        bool OnlyShowExplicitDetailTables { get; }

        InlineEditOptions InlineEditOptions { get; }

        CreateItemOptions CreateItemOptions { get; }

        GridCssClasses CssClasses { get; }

        IMasterDetailRelationship FindRelationshipConfiguration(Type detailType);
    }
}
