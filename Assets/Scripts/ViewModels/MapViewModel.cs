using UnityEngine.Rendering;

public class MapViewModel : IViewModel
{
    // (int: Rows, int: Columns)
    public ObservableProperty<Map> Map {get; set;} = new();

    public MapViewModel(Map map)
    {
        Map.SetAsMutable(map);
    }
}