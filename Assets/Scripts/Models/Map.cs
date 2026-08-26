using System.Text;

//! https://www.youtube.com/watch?v=WwkuAqObplU Data oriented design
public struct Map
{
    public int Id {get;}
    public int TotalRows {get;}
    public int TotalColumns {get;}

    public Map (int id, int rows, int columns)
    {
        Id = id;
        TotalRows = rows;
        TotalColumns = columns;
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"TotalRows: {TotalRows}");
        sb.AppendLine($"TotalColumns: {TotalColumns}");
        
        return sb.ToString();
    }
}