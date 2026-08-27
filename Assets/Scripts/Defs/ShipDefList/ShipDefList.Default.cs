using System.Drawing;

public static partial class ShipDefList
{
    public static ShipDef CreateCarrier()
    {
        return new ShipDef
        {
            Id = ShipIds.Carrier,
            Size = 5
        };
    }

    public static ShipDef CreateBattleship()
    {
        return new ShipDef
        {
            Id = ShipIds.Battleship,
            Size = 4
        };
    }

    public static ShipDef CreateDestroyer()
    {
        return new ShipDef
        {
            Id = ShipIds.Destroyer,
            Size = 3
        };
    }

    public static ShipDef CreateSubmarine()
    {
        return new ShipDef
        {
            Id = ShipIds.Submarine,
            Size = 3
        };
    }

    public static ShipDef CreatePatrolBoat()
    {
        return new ShipDef
        {
            Id = ShipIds.PatrolBoat,
            Size = 2
        };
    }
}