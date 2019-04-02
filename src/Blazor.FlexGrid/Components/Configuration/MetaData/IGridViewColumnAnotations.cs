using Blazor.FlexGrid.Components.Configuration.ValueFormatters;
using Blazor.FlexGrid.Components.Renderers;
using Blazor.FlexGrid.Permission;
using System;

namespace Blazor.FlexGrid.Components.Configuration.MetaData
{
    /// <summary>
    ///     Properties for grid column annotations accessed through
    /// </summary>
    public interface IGridViewColumnAnotations
    {
        string Caption { get; }

        string HeaderStyle { get; }

        int Order { get; }

        bool IsVisible { get; }

        bool IsSortable { get; }

        bool IsFilterable { get; }

        IValueFormatter ValueFormatter { get; }

        IRenderFragmentAdapter SpecialColumnValue { get; }

        Func<ICurrentUserPermission, bool> ReadPermissionRestrictionFunc { get; }

        Func<ICurrentUserPermission, bool> WritePermissionRestrictionFunc { get; }
    }
}
