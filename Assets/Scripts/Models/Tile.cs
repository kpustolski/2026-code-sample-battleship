using System.Text;

public struct Tile
{
    public Point Point {get;}

    public Tile(Point point)
    {
        Point = point;
    }

    public override string ToString()
    {
        return $"{Point}";
    }
}