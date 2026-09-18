public static partial class ShipDefList
{
    public static ShipDef CreateDefaultCarrier()
    {
        return new ShipDef
        {
            Id = ShipIds.Carrier,
            Group = ShipGroupIds.Default,
            Size = 5,
            DebugColor = (r: 148, g:0, b:211) // Violet
        };
    }

    public static ShipDef CreateDefaultBattleship()
    {
        return new ShipDef
        {
            Id = ShipIds.Battleship,
            Group = ShipGroupIds.Default,
            Size = 4,
            DebugColor = (r: 255, g:0, b:0) //red
        };
    }

    public static ShipDef CreateDefaultDestroyer()
    {
        return new ShipDef
        {
            Id = ShipIds.Destroyer,
            Group = ShipGroupIds.Default,
            Size = 3,
            DebugColor = (r: 0, g:0, b:255) // Blue
        };
    }

    public static ShipDef CreateDefaultSubmarine()
    {
        return new ShipDef
        {
            Id = ShipIds.Submarine,
            Group = ShipGroupIds.Default,
            Size = 3,
            DebugColor = (r: 0, g:255, b:0) // Green
        };
    }

    public static ShipDef CreateDefaultPatrolBoat()
    {
        return new ShipDef
        {
            Id = ShipIds.PatrolBoat,
            Group = ShipGroupIds.Default,
            Size = 2,
            DebugColor = (r: 255, g:255, b:0) // Yellow
        };
    }
}