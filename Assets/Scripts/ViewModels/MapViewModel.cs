using System;
using System.Collections.Generic;
using System.Text;

public class MapViewModel : IViewModel
{
    public ObservableList<Tile> TileList {get; set;} = new();

    public Map Map {get; set;}
    public int TotalRows => (Map != null) ? Map.TotalRows : 0;
    public int TotalColumns => (Map != null) ? Map.TotalColumns : 0;

    public MapViewModel(Map map)
    {
        Map = map;
        TileList.SetAsMutable(map._tileList);
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Map: [{Map}]");
        return sb.ToString();
    }
}