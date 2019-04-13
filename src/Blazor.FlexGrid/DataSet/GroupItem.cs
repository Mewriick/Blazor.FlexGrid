using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Blazor.FlexGrid.DataSet
{
    public class GroupItem<TItem> : GroupItem, IGrouping<object, TItem>
    {
        public IEnumerable<TItem> Items { get; set; }

        public override int Count => Items != null ? Items.Count() : 0;

        //Dynamic LINQ requires defaul ctor
        public GroupItem()
        {
            IsCollapsed = true;
            ItemType = typeof(TItem);
        }

        public GroupItem(object key, IEnumerable<TItem> items)
        {
            ItemType = typeof(TItem);
            Items = items;
            Key = key;
            IsCollapsed = true;
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

        public object Key { get; set; }

        public bool IsCollapsed { get; set; }

        public abstract int Count { get; }
    }

    public class GroupingKeyEqualityComparer : IEqualityComparer<object>
    {
        public new bool Equals(object x, object y)
        {
            return (x != null && y != null)
                  ? x.ToString() == y.ToString()
                  : x == null && y == null;
        }

        public int GetHashCode(object obj)
        {
            return obj.GetHashCode();
        }
    }
}
