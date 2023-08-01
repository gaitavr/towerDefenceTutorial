using System;

namespace Utils.Disposable
{
    public class Disposable<T> : IDisposable
    {
        private static readonly Action<T> EmptyDelegate = _ => { };

        private readonly Action<T> _dispose;
        private bool _isDisposed;

        public Disposable(T value, Action<T> dispose)
        {
            Value = value;
            _dispose = dispose;
        }
        
        public T Value { get; }
        
        public void Dispose()
        {
            if (_isDisposed)
                return;

            _isDisposed = true;
            _dispose(Value);
        }

        public static Disposable<T> Borrow(T value, Action<T> dispose) => new(value, dispose);

        public static Disposable<T> FakeBorrow(T value) => new(value, EmptyDelegate);
    }
}