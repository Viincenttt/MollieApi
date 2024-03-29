using System;
using System.Threading;

namespace Mollie.Api.Framework.Idempotency
{
    internal class AsyncLocalVariable<T> : IDisposable where T : class
    {
        private readonly AsyncLocal<T> _asyncLocal = new AsyncLocal<T>();

        public T Value
        {
            get => _asyncLocal.Value;
            set => _asyncLocal.Value = value;
        }

        public AsyncLocalVariable(T value)
        {
            _asyncLocal.Value = value;
        }

        public void Dispose()
        {
            _asyncLocal.Value = null;
        }
    }
}