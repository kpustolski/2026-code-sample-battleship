using UnityEngine.Tilemaps;
using UnityEngine;

public class TileViewModel : IViewModel
{
    // TODO: Do I need to know the point here?
    public ObservableProperty<Color> Color {get; set;} = new();

    public Point Point {get; private set;}

    public TileViewModel(Point point)
    {
        Point = point;
    }

    public void SetColor(Color color)
    {
        Color.SetAsMutable(color);
    }
}