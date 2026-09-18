using System;

/// <summary>
/// Template struct for creating strongly typed Ids.
/// Note: Would need to create JSON converter if I ever
/// needed to serialize this.
/// </summary>
public struct Id<T> : IEquatable<Id<T>>
{
    // Using a string variable type here instead
    // of something better memory-wise (like Guid)
    // so it's more readable at a glance.
    public string Value {get;}

    public Id(string value)
    {
        Value = value;
    }

    public override string ToString()
    {
        return Value;
    }

    public bool Equals(Id<T> other)
    {
        // The advantage to passing in StringComparison.Ordinal to be explicit in 
        // the comparison type.
        return string.Equals(Value, other.Value, StringComparison.Ordinal);
    }

    public override bool Equals(object obj)
    {
        if (obj is not Id<T> || obj is null)
        {
            return false;
        }
        
        Id<T> other = (Id<T>)obj;
        return Equals(other);
    }

    public override int GetHashCode()
    {
        // Must pass in StringComparison.Ordinal here since Equals()
        // specifies it explicitly when comparing strings.
        return Value.GetHashCode(StringComparison.Ordinal);
    }

#region Operator Overloads
    public static bool operator ==(Id<T> left, Id<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Id<T> left, Id<T> right)
    {
        return !left.Equals(right);
    }
#endregion
}