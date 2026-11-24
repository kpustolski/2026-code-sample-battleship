public class ShipDef
{
    public Id<Ship> Id {get; set;}
    public string Name {get; set;}
    public Id<ShipGroup> Group {get; set;}
    public int Size {get; set;}
}