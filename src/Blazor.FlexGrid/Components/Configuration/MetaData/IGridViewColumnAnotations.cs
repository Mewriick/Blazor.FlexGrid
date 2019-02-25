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

        int Order { get; }

        bool IsVisible { get; }

        bool IsSortable { get; }

        IValueFormatter<object> ValueFormatter { get; }

        IRenderFragmentAdapter<object> SpecialColumnValue { get; }

        Func<ICurrentUserPermission, bool> ReadPermissionRestrictionFunc { get; }

        Func<ICurrentUserPermission, bool> WritePermissionRestrictionFunc { get; }
    }
}
