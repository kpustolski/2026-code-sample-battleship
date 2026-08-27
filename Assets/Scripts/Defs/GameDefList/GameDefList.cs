using System.Collections.Generic;
using System.Linq;

public static partial class GameDefList
{
    public static GameDef GetDefById(Id<Game> id)
    {
        return Defs.FirstOrDefault(def => def.Id.Equals(id));
    }

    private static List<GameDef> Defs = new List<GameDef>()
    {
        CreateDefaultGameDef01()
    };
}