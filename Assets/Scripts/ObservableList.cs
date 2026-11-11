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

    public void ReplaceElement(T newValue)
    {
        List<T> oldValue = new List<T>(Value);
        var element = Value.FirstOrDefault(e => EqualityComparer<T>.Default.Equals(e, newValue));
        if (element != null)
        {
            int elementIndex = Value.IndexOf(element);
            Value.RemoveAt(elementIndex);
            Value.Insert(elementIndex, newValue);
        }

        this.DidChange?.Invoke(oldValue, Value);
    }

    public bool ContainsElement(T newValue)
    {
        var element = Value.FirstOrDefault(e => EqualityComparer<T>.Default.Equals(e, newValue));
        return element != null;
    }

    public void Subscribe(Action<List<T>, List<T>> callback)
    {
        if (callback == null)
        {
            this.LogError("Callback is null!");
            return;
        }

        callback(default, Value);
        DidChange += callback;
    }

    public void Unsubscribe(Action<List<T>, List<T>> callback)
    {
        if (callback == null)
        {
            this.LogError("Callback is null!");
            return;
        }

        DidChange -= callback;
    }
}