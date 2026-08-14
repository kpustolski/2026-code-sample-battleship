using System;
using System.Collections.Generic;

//? How was DidChange cleaned up when a view model was removed?
public class ObservableProperty<T>
{
    private T m_Value;

    public T Value
    {
        get
        {
            return m_Value;
        }
    }

    public event Action<T, T> DidChange;

    public void SetAsMutable(T newValue)
    {
        if (!EqualityComparer<T>.Default.Equals(newValue, m_Value))
        {
            T oldValue = m_Value;
            m_Value = newValue;
            this.DidChange?.Invoke(oldValue, newValue);
        }
    }
}