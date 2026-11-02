// We need this attribute in order to create drop downs of strongly
// typed ids in the Unity inspector.
[IdSource(typeof(GameMode))]
public static class GameModeIds
{
    public static readonly Id<GameMode> None = new(nameof(None));
    public static readonly Id<GameMode> Default = new(nameof(Default));
}