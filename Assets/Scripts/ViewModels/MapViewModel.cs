using UnityEngine.Rendering;

public class MapViewModel : IViewModel
{
    public ObservableProperty<int> TotalRows {get; set;}
    public ObservableProperty<int> TotalColumns {get; set;}

    public MapViewModel(int rows, int columns)
    {
        TotalRows.SetAsMutable(rows);
        TotalColumns.SetAsMutable(columns);
    }
}