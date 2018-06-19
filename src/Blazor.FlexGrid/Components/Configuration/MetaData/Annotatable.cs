using System;
using System.Collections.Generic;

namespace Blazor.FlexGrid.Components.Configuration.MetaData
{
    public class Annotatable : IAnnotatable
    {
        public object this[string name] => throw new NotImplementedException();

        public IAnnotation FindAnnotation(string name)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IAnnotation> GetAllAnnotations()
        {
            throw new NotImplementedException();
        }
    }
}
