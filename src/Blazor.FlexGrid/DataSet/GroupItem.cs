using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blazor.FlexGrid.DataSet
{
    public class GroupItem<TItem>: GroupItem, IGrouping<object, TItem>
    {
        
        //private IEnumerable<TItem> items;
        public IEnumerable<TItem> Items { get; set; }

        public override int Count => Items != null ? Items.Count() : 0;
                

        public GroupItem()
        {
            this.ItemType = typeof(TItem);
        }

        public GroupItem(object key, IEnumerable<TItem> items)
        {
            this.ItemType = typeof(TItem);
            this.Items = items;
            this.Key = key;
        }

        public GroupItem(IGrouping<object, TItem> groupItems)
        {
            this.ItemType = typeof(TItem);
            
            this.Items = groupItems;
            this.Key = groupItems.Key;
        }


        public IEnumerator<TItem> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Items.GetEnumerator();
        }
    }

    public abstract class GroupItem
    {
        public Type ItemType { get; set; }

        public object Key { get; protected set; }

        public bool IsCollapsed { get; set; }


        public abstract int Count { get; }
    }
}
