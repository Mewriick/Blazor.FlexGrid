namespace Blazor.FlexGrid.Triggers
{
    public interface IParamterChangedTrigger : ITrigger
    {
        /// <summary>
        /// Indicates that trigger is for paramter which recreate whole table data set
        /// </summary>
        bool RefreshPage { get; }
    }
}
