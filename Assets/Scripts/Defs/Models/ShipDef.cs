public class ShipDef
{
    public Id<Ship> Id {get; set;}
    public Id<ShipGroup> Group {get; set;}
    public int Size {get; set;}
    public (float r, float g, float b) DebugColor {get; set;}
}