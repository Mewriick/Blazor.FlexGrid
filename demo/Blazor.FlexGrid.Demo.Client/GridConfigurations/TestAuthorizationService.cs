using Blazor.FlexGrid.Permission;

namespace Blazor.FlexGrid.Demo.Client.GridConfigurations
{
    public class TestAuthorizationService : IAuthorizationService
    {
        public string UserToken => "Token";
    }
}
