using Blazor.FlexGrid.DataSet.Options;
using System.Collections.Generic;

namespace Blazor.FlexGrid.DataAdapters
{
    public interface IDetailDataAdapterVisitors
    {
        IEnumerable<IDataTableAdapterVisitor> GetVisitors(IMasterDetailRowArguments masterDetailRowArguments);
    }
}
