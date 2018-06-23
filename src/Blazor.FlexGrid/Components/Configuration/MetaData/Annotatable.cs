using System;
using System.Collections.Generic;

namespace Blazor.FlexGrid.Components.Configuration.MetaData
{
    public class Annotatable : IAnnotatable
    {
        private readonly SortedDictionary<string, Annotation> annotations;

        public object this[string name]
        {
            get => FindAnnotation(name).Value;
            set
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new ArgumentNullException(nameof(name));
                }

                if (value is null)
                {
                    RemoveAnnotation(name);
                }
                else
                {
                    SetAnnotation(name, value);
                }
            }

        }

        public Annotatable()
        {
            this.annotations = new SortedDictionary<string, Annotation>();
        }

        public Annotation FindAnnotation(string name)
            => annotations.TryGetValue(name, out var annotation)
                    ? annotation
                    : NullAnnotation.Instance;


        public IEnumerable<IAnnotation> GetAllAnnotations()
            => annotations.Values;

        public Annotation SetAnnotation(string name, object value)
        {
            var annotation = CreateAnnotation(name, value);

            annotations[name] = annotation;

            return annotation;
        }

        public Annotation RemoveAnnotation(string name)
        {
            var annotation = FindAnnotation(name);
            if (annotation is NullAnnotation)
            {
                return NullAnnotation.Instance;
            }

            annotations.Remove(name);

            return annotation;
        }

        IAnnotation IAnnotatable.FindAnnotation(string name)
            => FindAnnotation(name);

        protected virtual Annotation CreateAnnotation(string name, object value)
            => new Annotation(name, value);
    }
}
