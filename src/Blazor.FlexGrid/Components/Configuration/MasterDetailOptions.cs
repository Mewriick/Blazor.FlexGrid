namespace Blazor.FlexGrid.Components.Configuration
{
    public class MasterDetailOptions
    {
        public bool OnlyShowExplicitDetailTables { get; set; }
    }

    public class NullMasterDetailOptions : MasterDetailOptions
    {
        public static readonly NullMasterDetailOptions Instance = new NullMasterDetailOptions();
    }
}
