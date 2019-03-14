using Microsoft.AspNetCore.Components;
using System;

namespace Blazor.FlexGrid
{
    public class RefelctionHelper
    {
        public static EventCallback<T> CreateEventCallback<T>(object receiver, Action<T> callback)
            => new EventCallbackFactory().Create<T>(receiver, callback);
    }
}
