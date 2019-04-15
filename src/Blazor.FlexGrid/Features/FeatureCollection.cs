using System;
using System.Collections.Generic;
using System.Linq;

namespace Blazor.FlexGrid.Features
{
    public class FeatureCollection : IFeatureCollection
    {
        private Dictionary<Type, IFeature> features;

        public IFeature this[Type key]
        {
            get
            {
                if (key == null)
                {
                    throw new ArgumentNullException(nameof(key));
                }

                return features.TryGetValue(key, out var feature)
                    ? feature
                    : NullFeature.Instance;
            }
            set
            {
                if (key == null)
                {
                    throw new ArgumentNullException(nameof(key));
                }

                if (value != null)
                {
                    features[key] = value;
                }
            }
        }

        public FeatureCollection()
        {
            features = new Dictionary<Type, IFeature>();
        }

        public FeatureCollection(IEnumerable<IFeature> features)
        {
            this.features = features.ToDictionary(feature => feature.GetType(), feature => feature);
        }

        public TFeature Get<TFeature>() where TFeature : IFeature
        {
            var feature = this[typeof(TFeature)];
            if (feature == NullFeature.Instance)
            {
                return default;
            }

            return (TFeature)feature;
        }

        public void Set<TFeature>(TFeature instance) where TFeature : IFeature
        {
            this[typeof(TFeature)] = instance;
        }

        public bool Contains<TFeature>() where TFeature : IFeature
            => this[typeof(TFeature)] != NullFeature.Instance;
    }
}
