// We need this attribute in order to create drop downs of strongly
// typed ids in the Unity inspector.
[IdSource(typeof(ShipGroup))]
public static class ShipGroupIds
{
    public static readonly Id<ShipGroup> None = new(nameof(None));
    public static readonly Id<ShipGroup> Default = new(nameof(Default));
}