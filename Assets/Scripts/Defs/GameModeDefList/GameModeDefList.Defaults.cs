public partial class GameModeDefList
{
    public static GameModeDef CreateDefaultGameModeDef()
    {
        return new GameModeDef
        {
            Id = GameModeIds.Default,
            TotalRows = 5,
            TotalColumns = 5,
        };
    }
}