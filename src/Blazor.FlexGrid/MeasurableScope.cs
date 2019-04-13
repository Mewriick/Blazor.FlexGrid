using System;
using System.Diagnostics;

namespace Blazor.FlexGrid
{
    public class MeasurableScope : IDisposable
    {
        private bool disposedValue; // To detect redundant calls

        private readonly Stopwatch _stopwatch;
        private readonly Action<Stopwatch> _measuredResultAction;

        public MeasurableScope(Action<Stopwatch> measuredResultAction = default)
        {
            _stopwatch = new Stopwatch();
            _stopwatch.Start();
            _measuredResultAction = measuredResultAction;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _stopwatch.Stop();
                    _measuredResultAction?.Invoke(_stopwatch);
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
