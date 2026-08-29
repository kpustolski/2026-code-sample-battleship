using System.Text;

public struct Point
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
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"({X},{Y})");
        
        return sb.ToString();
    }
}
