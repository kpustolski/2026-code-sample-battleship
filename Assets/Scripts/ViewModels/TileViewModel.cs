using UnityEngine.Tilemaps;

public class TileViewModel : IViewModel
{
    public ObservableProperty<Tile> Tile {get; set;} = new();
    public ObservableProperty<float> WorldPositionX {get; set;} = new();
    public ObservableProperty<float> WorldPositionY {get; set;} = new();

    public Point Point => (Tile?.Value == null) ? new Point(0,0) : Tile.Value.Point;

    public TileViewModel(Tile tile, float worldPosX, float worldPosY)
    {
        Tile.SetAsMutable(tile);
        WorldPositionX.SetAsMutable(worldPosX);
        WorldPositionY.SetAsMutable(worldPosY);
    }
}