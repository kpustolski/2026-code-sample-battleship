using System;
using System.Collections;
using System.Text;

public struct Point : IEquatable<Point>
{
    public float X {get;}
    public float Y {get;}

    public Point(float xCord, float yCord)
    {
        X = xCord;
        Y = yCord;
    }

    public override string ToString()
    {   
        return $"({X},{Y})";
    }

    public bool Equals(Point other)
    {
        return this.X == other.X && this.Y == other.Y;
    }

    public override bool Equals(object obj)
    {
        if (obj is not Point|| obj is null)
        {
            return false;
        }
        
        Point other = (Point)obj;
        return Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

#region Operator Overloads
    public static bool operator ==(Point left, Point right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Point left, Point right)
    {
        return !left.Equals(right);
    }
#endregion
}
