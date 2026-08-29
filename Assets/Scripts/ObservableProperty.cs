using System;
using System.Collections.Generic;

//? How was DidChange cleaned up when a view model was removed?
public class ObservableProperty<T>
{
    private T _value;

    public T Value
    {
        get
        {
            return _value;
        }
    }

    public bool HasValue => Value != null;
    public event Action<T, T> DidChange;

    public void SetAsMutable(T newValue)
    {
        if (!EqualityComparer<T>.Default.Equals(newValue, _value))
        {
            T oldValue = _value;
            _value = newValue;
            DidChange?.Invoke(oldValue, newValue);
        }
    }

    public IDisposable Subscribe(Action<T, T> callback)
    {
        callback(default, Value);
        DidChange += callback;
        return new Unsubscriber(() => DidChange -= callback);
    }

    // Note from Calude
    private sealed class Unsubscriber : IDisposable
    {
        private readonly Action _unsubscribe;
        public Unsubscriber(Action unsubscribe) => _unsubscribe = unsubscribe;
        public void Dispose() => _unsubscribe();
    }
}