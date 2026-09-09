public partial class GameModeDefList
{
    public static GameModeDef CreateDefaultGameModeDef()
    {
        return new GameModeDef
        {
            Id = GameModeIds.Default,
            ShipGroup = ShipGroupIds.Default,
            TotalRows = 10,
            TotalColumns = 10,
        };
    }
}