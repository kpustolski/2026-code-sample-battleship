using System;

/// <summary>
/// Data Transfer Object for Ships.
/// </summary>
public struct ShipDto: IEquatable<ShipDto>
{
    public Id<Ship> ShipId {get; private set;}
    public int Health { get; private set;}

    public ShipDto(Id<Ship> shipId, int health)
    {
        ShipId = shipId;
        Health = health;
    }

    public override string ToString()
    {   
        return $"Id: {ShipId} | Health {Health}";
    }
    
    public bool Equals(ShipDto other)
    {
        return this.ShipId == other.ShipId;
    }

    public override bool Equals(object obj)
    {
        if (obj is not ShipDto || obj is null)
        {
            return false;
        }
        
        ShipDto other = (ShipDto)obj;
        return Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(ShipId, Health);
    }

#region Operator Overloads
    public static bool operator ==(ShipDto left, ShipDto right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(ShipDto left, ShipDto right)
    {
        return !left.Equals(right);
    }
#endregion
}