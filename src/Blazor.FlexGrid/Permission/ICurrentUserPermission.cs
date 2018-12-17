namespace Blazor.FlexGrid.Permission
{
    public interface ICurrentUserPermission
    {
        bool IsInRole(string roleName);

        bool HasClaim(string claimName);
    }
}
