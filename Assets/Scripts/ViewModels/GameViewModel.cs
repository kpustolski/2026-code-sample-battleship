
public class GameViewModel : IViewModel
{
    public ObservableProperty<Map> Map {get; set;} = new();

    public string GameId {get; set;}

    public GameViewModel(string gameId, Map map)
    {
        GameId = gameId;
        Map.SetAsMutable(map);
    }
}