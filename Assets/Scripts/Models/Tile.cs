using System.Text;

public struct Tile
{
    public int Id {get;}
    public Point Point {get;}

    public Tile(int id, Point point)
    {
        Id = id;
        Point = point;
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Id: {Id}");
        sb.AppendLine($"Point: {Id}");

        return sb.ToString();
    }
}