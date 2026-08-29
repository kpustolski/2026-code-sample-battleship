using UnityEngine.Tilemaps;

public class TileViewModel : IViewModel
{
    public ObservableProperty<Tile> Tile {get; set;} = new();

    public Point Point => Tile.HasValue ? new Point(0,0) : Tile.Value.Point;

    public TileViewModel(Tile tile)
    {
        Tile.SetAsMutable(tile);
    }
}