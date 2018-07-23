using System;

namespace Blazor.FlexGrid.Components.Configuration.MetaData
{
    public class MasterDetailRelationship : Annotatable, IMasterDetailRelationship
    {
        public string ForeignPropertyName { get; }

        public string MasterPropertyName { get; }

        public MasterDetailRelationship(string masterPropertyName, string foreignPropertyName)
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
