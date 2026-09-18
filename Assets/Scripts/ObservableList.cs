using System;
using System.Collections.Generic;
using System.Linq;

public class ObservableList<T>
{
    private List<T> _value = new List<T>();

    public List<T> Value
    {
        get
        {
            return _value;
        }
    }

    public bool HasValue => Value != null;
    public event Action<List<T>, List<T>> DidChange;

    //TODO: Must test. Is there a better way to compare?
    public void SetAsMutable(List<T> newValue)
    {
        List<T> oldValue = new List<T>(Value);
        if (!EqualityComparer<List<T>>.Default.Equals(oldValue, newValue))
        {
            _value = new List<T>(newValue);
            this.DidChange?.Invoke(oldValue, newValue);
        }
    }

    public IDisposable Subscribe(Action<List<T>, List<T>> callback)
    {
        callback(default, Value);
        DidChange += callback;
        return new Unsubscriber(() => DidChange -= callback);
    }

    //TODO: Note from Calude
    private sealed class Unsubscriber : IDisposable
    {
        private Action _unsubscribe;

        public Unsubscriber(Action unsubscribe)
        {
            _unsubscribe = unsubscribe;
        }

        public void Dispose()
        {
            _unsubscribe?.Invoke();
        }
    }
}