namespace Blazor.FlexGrid.Components.Configuration.MetaData
{
    public interface IMasterDetailRelationship : IAnnotatable
    {
        bool IsOwnCollection { get; }

        IMasterDetailConnection MasterDetailConnection { get; }
    }

    public interface IMasterDetailConnection
    {
        string MasterPropertyName { get; }

        string ForeignPropertyName { get; }
    }
}
