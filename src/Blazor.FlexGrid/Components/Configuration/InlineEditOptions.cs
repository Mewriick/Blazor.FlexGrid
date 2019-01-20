using Blazor.FlexGrid.Permission;
using System;

namespace Blazor.FlexGrid.Components.Configuration
{
    public class InlineEditOptions
    {
        public virtual bool InlineEditIsAllowed => true;

        public bool AllowDeleting { get; set; }

        public Func<ICurrentUserPermission, bool> DeletePermissionRestriction { get; set; } = perm => true;
    }

    public class NullInlineEditOptions : InlineEditOptions
    {
        public static NullInlineEditOptions Instance = new NullInlineEditOptions();

        public override bool InlineEditIsAllowed => false;
    }
}
