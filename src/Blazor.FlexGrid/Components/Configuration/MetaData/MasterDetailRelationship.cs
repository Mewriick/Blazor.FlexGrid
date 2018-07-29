using System;

namespace Blazor.FlexGrid.Components.Configuration.MetaData
{
    public class MasterDetailRelationship : Annotatable, IMasterDetailRelationship
    {
        public bool IsOwnCollection { get; }

        public IMasterDetailConnection MasterDetailConnection { get; }

        public MasterDetailRelationship(string masterPropertyName, string foreignPropertyName)
        {
            MasterDetailConnection = new MasterDetailConnection(masterPropertyName, foreignPropertyName);
            IsOwnCollection = false;
        }

        public MasterDetailRelationship()
        {
            IsOwnCollection = true;
        }
    }

    public class MasterDetailConnection : IMasterDetailConnection
    {
        public string MasterPropertyName { get; }

        public string ForeignPropertyName { get; }

        public MasterDetailConnection(string masterPropertyName, string foreignPropertyName)
        {
            if (string.IsNullOrWhiteSpace(foreignPropertyName))
            {
                throw new ArgumentNullException(nameof(foreignPropertyName));
            }

            if (string.IsNullOrWhiteSpace(masterPropertyName))
            {
                throw new ArgumentNullException(nameof(masterPropertyName));
            }

            ForeignPropertyName = foreignPropertyName;
            MasterPropertyName = masterPropertyName;
        }
    }
}
