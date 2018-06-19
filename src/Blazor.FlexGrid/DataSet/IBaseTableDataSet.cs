using System.Collections;
using System.Collections.Generic;

namespace Blazor.FlexGrid.DataSet
{
    /// <summary>
    /// Define contract for table dataset
    /// </summary>
    /// <typeparam name="TItem">The type of the <see cref="Items" /> elements.</typeparam>
    public interface IBaseTableDataSet<TItem> : IBaseTableDataSet
    {
        new IList<TItem> Items { get; }
    }


    /// <summary>
    /// Define contract for table dataset
    /// </summary>
    public interface IBaseTableDataSet
    {
        IList Items { get; }
    }
}
