using Blazor.FlexGrid.Permission;

namespace Blazor.Components.Demo.FlexGrid.GridConfigurations
{
    public class TestCurrentUserPermission : ICurrentUserPermission
    {
        public bool HasClaim(string claimName)
        {
            if (claimName == "Test")
            {
                return true;
            }

            return false;
        }

        public bool IsInRole(string roleName)
        {
            if (roleName == "TestRole")
            {
                return true;
            }

            return false;
        }
    }
}
