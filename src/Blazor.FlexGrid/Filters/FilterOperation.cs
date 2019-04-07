using System;

namespace Blazor.FlexGrid.Filters
{
    [Flags]
    public enum FilterOperation
    {
        None = 0,
        Equal = 1,
        LessThan = 2,
        LessThanOrEqual = 4,
        GreaterThan = 8,
        GreaterThanOrEqual = 16,
        NotEqual = 32,
        Contains = 64,
        StartsWith = 128,
        EndsWith = 256
    }
}
