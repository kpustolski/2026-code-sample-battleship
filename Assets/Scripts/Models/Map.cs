using System;
using System.Collections.Generic;
using System.Text;

//! https://www.youtube.com/watch?v=WwkuAqObplU Data oriented design
public class Map
{
    public int Id {get;}
    public int TotalRows {get;}
    public int TotalColumns {get;}
    public List<Tile> _tileList = new List<Tile>();
    public List<Tile> TileList => _tileList;

    public Map (int id, int rows, int columns)
    {
        Id = id;
        TotalRows = rows;
        TotalColumns = columns;

        CreateTileList();
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append($"Id: {Id}, ");
        sb.Append($"TotalRows: {TotalRows}, ");
        sb.Append($"TotalColumns: {TotalColumns}, ");
        sb.Append($"TileList.Count: {TileList.Count}");
        return sb.ToString();
    }

    public void CreateTileList()
    {
        if (TotalRows != TotalColumns)
        {
            this.LogError("The number of rows must equal the number of columns!");
            return;
        }

        for (int i = 0; i < TotalRows; i++)
        {
            for (int j = 0; j < TotalColumns; j++)
            {
                Point point = new Point(i,j);
                Tile newTile = new Tile(point);

                _tileList.Add(newTile);
            }
        }
    }
}