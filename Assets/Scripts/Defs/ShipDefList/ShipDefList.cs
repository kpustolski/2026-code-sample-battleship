using System.Collections.Generic;
using System.Linq;

public static partial class ShipDefList
{
    public static ShipDef GetDefById(Id<Ship> id)
    {
        return Defs.FirstOrDefault(def => def.Id.Equals(id));
    }

    public static List<ShipDef> GetAllShipsInGroupById(Id<ShipGroup> id)
    {
        return Defs.FindAll(def => def.Group.Equals(id));
    }

    private static List<ShipDef> Defs = new List<ShipDef>()
    {
        CreateDefaultCarrier(),
        CreateDefaultBattleship(),
        CreateDefaultDestroyer(),
        CreateDefaultSubmarine(),
        CreateDefaultPatrolBoat()
    };
}