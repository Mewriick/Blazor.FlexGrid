using Blazor.FlexGrid.Components.Configuration.MetaData;
using System;
using System.Collections.Generic;

namespace Blazor.FlexGrid.Permission
{
    public class PermissionContext
    {
        private readonly ICurrentUserPermission currentUserPermission;
        private readonly IEntityType gridEntityConfiguration;
        private readonly Dictionary<string, PermissionAccess> columnPermissions;

        public bool HasDeleteItemPermission { get; }

        public bool HasCreateItemPermission { get; }

        public PermissionContext(ICurrentUserPermission currentUserPermission, IEntityType gridEntityConfiguration)
        {
            this.currentUserPermission = currentUserPermission ?? throw new ArgumentNullException(nameof(currentUserPermission));
            this.gridEntityConfiguration = gridEntityConfiguration ?? throw new ArgumentNullException(nameof(gridEntityConfiguration));
            columnPermissions = new Dictionary<string, PermissionAccess>();

            HasDeleteItemPermission = new GridAnotations(gridEntityConfiguration)
                .InlineEditOptions.DeletePermissionRestriction(currentUserPermission);

            HasCreateItemPermission = new GridAnotations(gridEntityConfiguration)
                .CreateItemOptions.CreatePermissionRestriction(currentUserPermission);
        }

        public bool HasCurrentUserReadPermission(string columnName)
        {
            if (columnPermissions.TryGetValue(columnName, out var permission))
            {
                permission.HasFlag(PermissionAccess.Read);
            }

            return true;
        }

        public bool HasCurrentUserWritePermission(string columnName)
        {
            if (columnPermissions.TryGetValue(columnName, out var permission))
            {
                permission.HasFlag(PermissionAccess.Write);
            }

            return true;
        }

        public void ResolveColumnPermission(IGridViewColumnAnotations columnConfig, string columnName)
        {
            var permissionAccess = PermissionAccess.None;
            if (columnConfig is null)
            {
                permissionAccess |= PermissionAccess.Read | PermissionAccess.Write;
            }
            else
            {
                permissionAccess |= columnConfig.ReadPermissionRestrictionFunc(currentUserPermission)
                   ? PermissionAccess.Read
                   : PermissionAccess.None;

                permissionAccess |= columnConfig.WritePermissionRestrictionFunc(currentUserPermission)
                   ? PermissionAccess.Write
                   : PermissionAccess.None;
            }

            columnPermissions.Add(columnName, permissionAccess);
        }
    }
}
