public static partial class ShipDefList
{
    public static ShipDef CreateDefaultCarrier()
    {
        return new ShipDef
        {
            Id = ShipIds.Carrier,
            Name = "Carrier",
            Group = ShipGroupIds.Default,
            Size = 5,
        };
    }

    public static ShipDef CreateDefaultBattleship()
    {
        return new ShipDef
        {
            Id = ShipIds.Battleship,
            Name = "Battleship",
            Group = ShipGroupIds.Default,
            Size = 4,
        };
    }

    public static ShipDef CreateDefaultDestroyer()
    {
        return new ShipDef
        {
            Id = ShipIds.Destroyer,
            Name = "Destroyer",
            Group = ShipGroupIds.Default,
            Size = 3,
        };
    }

    public static ShipDef CreateDefaultSubmarine()
    {
        return new ShipDef
        {
            Id = ShipIds.Submarine,
            Name = "Submarine",           
            Group = ShipGroupIds.Default,
            Size = 3,
        };
    }

    public static ShipDef CreateDefaultPatrolBoat()
    {
        return new ShipDef
        {
            Id = ShipIds.PatrolBoat,
            Name = "Patrol Boat",
            Group = ShipGroupIds.Default,
            Size = 2,
        };
    }
}