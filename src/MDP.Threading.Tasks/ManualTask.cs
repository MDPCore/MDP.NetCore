using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MDP.Threading.Tasks
{
    public class ManualTask : IDisposable
    {
        // Fields
        private readonly TaskCompletionSource _completionSource = new TaskCompletionSource();

        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        private bool _disposed = false;


        // Methods
        public async Task WaitAsync()
        {
            // Execute
            try
            {
                // WaitAsync
                await _completionSource.Task.WaitAsync(_cancellationTokenSource.Token);
            }
            catch (Exception exception)
            {
                // TrySetException
                _completionSource.TrySetException(exception);

                // throw
                throw;
            }
        }

        public async Task WaitAsync(TimeSpan timeout)
        {
            // Execute
            try
            {
                // WaitAsync
                await _completionSource.Task.WaitAsync(timeout, _cancellationTokenSource.Token);
            }
            catch (Exception exception)
            {
                // TrySetException
                _completionSource.TrySetException(exception);

                // throw
                throw;
            }
        }

        public bool TrySetResult()
        {
            // TrySetResult
            return _completionSource.TrySetResult();
        }

        public bool TrySetException(Exception exception)
        {
            // TrySetException
            return _completionSource.TrySetException(exception);
        }

        public void Cancel()
        {
            // Sync
            lock (_cancellationTokenSource)
            {
                // Require
                if (_disposed == true) return;
            }

            // Cancel
            _cancellationTokenSource.Cancel();
        }

        public void Dispose()
        {
            // Sync
            lock (_cancellationTokenSource)
            {
                // Require
                if (_disposed == true) return;
                _disposed = true;
            }

            // Dispose
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }
    }

    public class ManualTask<T> : IDisposable
    {
        // Fields
        private readonly TaskCompletionSource<T> _completionSource = new TaskCompletionSource<T>();

        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        private bool _disposed = false;


        // Methods
        public async Task<T> WaitAsync()
        {
            // Execute
            try
            {
                // WaitAsync
                return await _completionSource.Task.WaitAsync(_cancellationTokenSource.Token);
            }
            catch (Exception exception)
            {
                // TrySetException
                _completionSource.TrySetException(exception);

                // throw
                throw;
            }
        }

        public async Task<T> WaitAsync(TimeSpan timeout)
        {
            // Execute
            try
            {
                // WaitAsync
                return await _completionSource.Task.WaitAsync(timeout, _cancellationTokenSource.Token);
            }
            catch (Exception exception)
            {
                // TrySetException
                _completionSource.TrySetException(exception);

                // throw
                throw;
            }          
        }

        public bool TrySetResult(T result)
        {
            // TrySetResult
            return _completionSource.TrySetResult(result);
        }

        public bool TrySetException(Exception exception)
        {
            // TrySetException
            return _completionSource.TrySetException(exception);
        }

        public void Cancel()
        {
            // Sync
            lock (_cancellationTokenSource)
            {
                // Require
                if (_disposed == true) return;
            }

            // Cancel
            _cancellationTokenSource.Cancel();
        }

        public void Dispose()
        {
            // Sync
            lock (_cancellationTokenSource)
            {
                // Require
                if (_disposed == true) return;
                _disposed = true;
            }

            // Dispose
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }
    }
}
