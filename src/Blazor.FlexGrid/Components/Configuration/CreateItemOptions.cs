using Blazor.FlexGrid.Permission;
using System;

namespace Blazor.FlexGrid.Components.Configuration
{
    public class CreateItemOptions
    {
        public static readonly string CreateItemModalName = "createItemModal";
        public static readonly string CreateItemModalSizeDiv = "createItemModalSize";

        public virtual bool IsCreateItemAllowed => true;

        public string CreateUri { get; set; } = string.Empty;

        public Type ModelType { get; set; }

        public Type OutputDtoType { get; set; }

        public Func<ICurrentUserPermission, bool> CreatePermissionRestriction { get; set; } = perm => true;
    }

    public class NullCreateItemOptions : CreateItemOptions
    {
        public static readonly NullCreateItemOptions Instance = new NullCreateItemOptions();

        public override bool IsCreateItemAllowed => false;
    }
}
