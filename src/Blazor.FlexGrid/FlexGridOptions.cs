namespace Blazor.FlexGrid
{
    public class FlexGridOptions
    {
        public bool IsServerSideBlazorApp { get; set; } = false;

        public bool UseAuthorizationForHttpRequests { get; set; }

        public string BaseServerAddress { get; set; }
    }
}
