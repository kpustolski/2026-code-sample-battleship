using System.Collections.Generic;

//? How to not make this static?
public static partial class ShipDefList
{
    public static List<ShipDef> Defs = new List<ShipDef>()
    {
        CreateCarrier(),
        CreateBattleship(),
        CreateDestroyer(),
        CreateSubmarine(),
        CreatePatrolBoat()
    };
}