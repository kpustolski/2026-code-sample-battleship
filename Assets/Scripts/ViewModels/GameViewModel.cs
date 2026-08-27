
public class GameViewModel : IViewModel
{
    public ObservableProperty<Map> Map {get; set;} = new();

    public Id<Game> GameId {get; set;}

    public GameViewModel(Id<Game> gameId, Map map)
    {
        GameId = gameId;
        Map.SetAsMutable(map);
    }
}