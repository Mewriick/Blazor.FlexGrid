using System;
using System.Collections.Generic;

namespace Blazor.FlexGrid.Tests.Mocks
{
    internal class TestServiceProvider : IServiceProvider
    {
        private readonly Dictionary<Type, Func<object>> _factories
            = new Dictionary<Type, Func<object>>();

        public object GetService(Type serviceType)
            => _factories.TryGetValue(serviceType, out var factory)
                ? factory()
                : null;

        internal void AddService<T>(T value)
            => _factories.Add(typeof(T), () => value);
    }
}