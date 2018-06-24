using System.Collections.Generic;

namespace Blazor.FlexGrid.Components.Configuration.MetaData
{
    public class EntityTypeComparer : IComparer<EntityType>
    {
        public int Compare(EntityType x, EntityType y)
        {
            return x.Name.CompareTo(y.Name);
        }
    }
}
