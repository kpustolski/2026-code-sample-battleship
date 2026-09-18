// TODO: Unit test to check for duplicates.
public static class ShipIds
{
    public static readonly Id<Ship> None = new(nameof(None));
    public static readonly Id<Ship> Carrier = new(nameof(Carrier));
    public static readonly Id<Ship> Battleship = new(nameof(Battleship));
    public static readonly Id<Ship> Destroyer = new(nameof(Destroyer));
    public static readonly Id<Ship> Submarine = new(nameof(Submarine));
    public static readonly Id<Ship> PatrolBoat = new(nameof(PatrolBoat));
}