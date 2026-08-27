
public class GameViewModel : IViewModel
{
    public ObservableProperty<Map> Map {get; set;} = new();

    public Id<GameMode> GameModeId {get; set;}

    public GameViewModel(Id<GameMode> gameModeId, Map map)
    {
        GameModeId = gameModeId;
        Map.SetAsMutable(map);
    }
}