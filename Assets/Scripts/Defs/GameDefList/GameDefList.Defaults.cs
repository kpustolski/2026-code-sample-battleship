public partial class GameDefList
{
    public static GameDef CreateDefaultGameDef01()
    {
        return new GameDef
        {
            Id = GameIds.Default01,
            TotalRows = 5,
            TotalColumns = 5,
        };
    }
}