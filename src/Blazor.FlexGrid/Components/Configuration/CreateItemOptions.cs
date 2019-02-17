using Blazor.FlexGrid.Permission;
using System;

namespace Blazor.FlexGrid.Components.Configuration
{
    public class CreateItemOptions
    {
        public static readonly string CreateItemModalName = "createItemModal";

        public virtual bool IsCreateItemAllowed => true;

        public Type ItemType { get; set; }

        public Func<ICurrentUserPermission, bool> CreatePermissionRestriction { get; set; } = perm => true;
    }

    public class NullCreateItemOptions : CreateItemOptions
    {
        public static readonly NullCreateItemOptions Instance = new NullCreateItemOptions();

        public override bool IsCreateItemAllowed => false;
    }
}
