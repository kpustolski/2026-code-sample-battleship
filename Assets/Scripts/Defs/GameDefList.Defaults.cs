public partial class GameDefList
{
    public static GameDef CreateDefaultDef01()
    {
        return new GameDef
        {
            Id = "Default_01",
            TotalRows = 5,
            TotalColumns = 5,
        };
    }
}