using System.Collections.Generic;
using System.Linq;

public static partial class GameModeDefList
{
    public static GameModeDef GetDefById(Id<GameMode> id)
    {
        return Defs.FirstOrDefault(def => def.Id.Equals(id));
    }

    private static List<GameModeDef> Defs = new List<GameModeDef>()
    {
        CreateDefaultGameModeDef()
    };
}