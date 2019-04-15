namespace Blazor.FlexGrid.Components.Configuration
{
    public class GlobalGroupingOptions
    {
        internal bool IsGroupingEnabled { get; set; } = true;

        public int GroupPageSize { get; set; } = 10;
    }

    public class NullGlobalGroupingOptions : GlobalGroupingOptions
    {
        public static NullGlobalGroupingOptions Instance = new NullGlobalGroupingOptions();

        public NullGlobalGroupingOptions()
        {
            IsGroupingEnabled = false;
        }
    }
}
