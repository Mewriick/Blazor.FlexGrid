namespace Blazor.FlexGrid.Permission
{
    public class NullCurrentUserPermission : ICurrentUserPermission
    {
        public bool HasClaim(string claimName)
            => true;

        public bool IsInRole(string roleName)
            => true;
    }
}
