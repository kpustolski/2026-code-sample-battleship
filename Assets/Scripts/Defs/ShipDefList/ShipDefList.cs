using System.Collections.Generic;
using System.Linq;

public static partial class ShipDefList
{
    public static ShipDef GetDefById(Id<Ship> id)
    {
        return Defs.FirstOrDefault(def => def.Id.Equals(id));
    }

    private static List<ShipDef> Defs = new List<ShipDef>()
    {
        CreateCarrier(),
        CreateBattleship(),
        CreateDestroyer(),
        CreateSubmarine(),
        CreatePatrolBoat()
    };
}