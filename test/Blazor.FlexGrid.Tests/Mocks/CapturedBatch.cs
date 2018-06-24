using Microsoft.AspNetCore.Blazor.RenderTree;
using System.Collections.Generic;

namespace Blazor.FlexGrid.Tests.Mocks
{
    public class CapturedBatch
    {
        public IDictionary<int, List<RenderTreeDiff>> DiffsByComponentId { get; }
            = new Dictionary<int, List<RenderTreeDiff>>();

        public IList<RenderTreeDiff> DiffsInOrder { get; }
            = new List<RenderTreeDiff>();

        public IList<int> DisposedComponentIDs { get; set; }
        public RenderTreeFrame[] ReferenceFrames { get; set; }

        internal void AddDiff(RenderTreeDiff diff)
        {
            var componentId = diff.ComponentId;
            if (!DiffsByComponentId.ContainsKey(componentId))
            {
                DiffsByComponentId.Add(componentId, new List<RenderTreeDiff>());
            }

            // Clone the diff, because its underlying storage will get reused in subsequent batches
            /*var diffClone = new RenderTreeDiff(
                diff.ComponentId,
                new ArraySegment<RenderTreeEdit>(diff.Edits.ToArray()));
            DiffsByComponentId[componentId].Add(diffClone);
            DiffsInOrder.Add(diffClone);*/
        }
    }
}