using System.Drawing;

public static partial class ShipDefList
{
    public static ShipDef CreateCarrier()
    {
        return new ShipDef
        {
            Id = "Carrier",
            Size = 5
        };
    }

    public static ShipDef CreateBattleship()
    {
        return new ShipDef
        {
            Id = "Battleship",
            Size = 4
        };
    }

    public static ShipDef CreateDestroyer()
    {
        return new ShipDef
        {
            Id = "Destroyer",
            Size = 3
        };
    }

    public static ShipDef CreateSubmarine()
    {
        return new ShipDef
        {
            Id = "Submarine",
            Size = 3
        };
    }

    public static ShipDef CreatePatrolBoat()
    {
        return new ShipDef
        {
            Id = "PatrolBoat",
            Size = 2
        };
    }
}