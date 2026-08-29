using System.Drawing;

public static partial class ShipDefList
{
    public static ShipDef CreateDefaultCarrier()
    {
        return new ShipDef
        {
            Id = ShipIds.Carrier,
            Group = ShipGroupIds.Default,
            Size = 5
        };
    }

    public static ShipDef CreateDefaultBattleship()
    {
        return new ShipDef
        {
            Id = ShipIds.Battleship,
            Group = ShipGroupIds.Default,
            Size = 4
        };
    }

    public static ShipDef CreateDefaultDestroyer()
    {
        return new ShipDef
        {
            Id = ShipIds.Destroyer,
            Group = ShipGroupIds.Default,
            Size = 3
        };
    }

    public static ShipDef CreateDefaultSubmarine()
    {
        return new ShipDef
        {
            Id = ShipIds.Submarine,
            Group = ShipGroupIds.Default,
            Size = 3
        };
    }

    public static ShipDef CreateDefaultPatrolBoat()
    {
        return new ShipDef
        {
            Id = ShipIds.PatrolBoat,
            Size = 2
        };
    }
}