using System.Collections.Generic;

//? How to not make this static?
public static partial class GameDefList
{
    public static List<GameDef> Defs = new List<GameDef>()
    {
        CreateDefaultGameDef01()
    };
}