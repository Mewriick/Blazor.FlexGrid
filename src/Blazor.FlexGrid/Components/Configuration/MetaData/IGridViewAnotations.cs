﻿using System;

namespace Blazor.FlexGrid.Components.Configuration.MetaData
{
    public interface IGridViewAnotations
    {
        bool IsMasterTable { get; }

        bool OnlyShowExplicitProperties { get; }

        bool RenderHeaderWithEmtyItemsMessage { get; }

        string EmptyItemsMessage { get; }

        bool PreserveFiltering { get; }

        bool PreservePagination { get; }

        InlineEditOptions InlineEditOptions { get; }

        MasterDetailOptions MasterDetailOptions { get; }

        CreateItemOptions CreateItemOptions { get; }

        DeleteItemOptions DeleteItemOptions { get; }

        GridCssClasses CssClasses { get; }

        GlobalGroupingOptions GroupingOptions { get; }

        IMasterDetailRelationship FindRelationshipConfiguration(Type detailType);
    }
}
