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

    public void Subscribe(Action<T, T> callback)
    {
        if (callback == null)
        {
            this.LogError("Callback is null!");
            return;
        }

        callback(default, Value);
        DidChange += callback;
    }

    public void Unsubscribe(Action<T, T> callback)
    {
        if (callback == null)
        {
            this.LogError("Callback is null!");
            return;
        }

        DidChange -= callback;
    }
}