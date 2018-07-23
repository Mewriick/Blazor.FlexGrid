namespace Blazor.FlexGrid.Components.Configuration.MetaData
{
    public interface IMasterDetailRelationship : IAnnotatable
    {
        string MasterPropertyName { get; }

        string ForeignPropertyName { get; }
    }
}
