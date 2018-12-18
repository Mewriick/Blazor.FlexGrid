using System;

namespace Blazor.FlexGrid.Permission
{
    [Flags]
    public enum PermissionAccess
    {
        None = 0,
        Read = 1,
        Write = 2
    }
}
