using System;

public struct ShipHealth : IEquatable<ShipHealth>
{
    public Id<Ship> ShipId {get; private set;}
    public int Health { get; private set;}

    public ShipHealth(Id<Ship> shipId, int health)
    {
        ShipId = shipId;
        Health = health;
    }

    public override string ToString()
    {   
        return $"Id: {ShipId} | Health {Health}";
    }
    
    public bool Equals(ShipHealth other)
    {
        return this.ShipId == other.ShipId;
    }

    public override bool Equals(object obj)
    {
        if (obj is not ShipHealth || obj is null)
        {
            return false;
        }
        
        ShipHealth other = (ShipHealth)obj;
        return Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(ShipId, Health);
    }

#region Operator Overloads
    public static bool operator ==(ShipHealth left, ShipHealth right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(ShipHealth left, ShipHealth right)
    {
        return !left.Equals(right);
    }
#endregion
}