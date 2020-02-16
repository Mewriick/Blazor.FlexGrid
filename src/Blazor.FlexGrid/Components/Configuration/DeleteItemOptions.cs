namespace Blazor.FlexGrid.Components.Configuration
{
    public class DeleteItemOptions
    {
        public const string DialogName = "deleteDialog";

        public bool UseConfirmationDialog { get; set; }
    }

    public class DefaulDeleteItemOptions : DeleteItemOptions
    {
        public DefaulDeleteItemOptions()
            : base()
        {
            UseConfirmationDialog = true;
        }
    }
}
