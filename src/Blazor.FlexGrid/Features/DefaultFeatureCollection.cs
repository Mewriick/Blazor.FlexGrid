using System.Collections.Generic;

namespace Blazor.FlexGrid.Features
{
    public class DefaultFeatureCollection
    {
        public static readonly IEnumerable<IFeature> AllFeatures = new List<IFeature>() { new GroupingFeature(), new TableHeaderFeature(),
            new PaginationFeature(), new CreateItemFeature(), new FilteringFeature() };

        public static readonly IEnumerable<IFeature> GroupedItemsFeatures = new List<IFeature> { new PaginationFeature() };
    }
}

