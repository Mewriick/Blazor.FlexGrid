namespace Blazor.FlexGrid.Permission
{
    public class NullAuthorizationService : IAuthorizationService
    {
        public string UserToken => string.Empty;
    }
}
