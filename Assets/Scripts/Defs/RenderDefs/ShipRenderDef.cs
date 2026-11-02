using UnityEngine;

/// <summary>
/// Render def scriptable object specific to each Ship.
/// </summary>
[CreateAssetMenu(fileName = "ShipRenderDef", menuName = "Scriptable Objects/ShipRenderDef")]
public class ShipRenderDef : ScriptableObject
{
    [SerializeField]
    private Id<Ship> _shipId = ShipIds.Battleship;
    [SerializeField]
    private Sprite _shipSprite;

    //TODO: Remove when finished project
    [Header("Debug Variables")]
    [SerializeField]
    private Color _debugColor;

    public Id<Ship> ShipId => _shipId;
    public Sprite ShipSprite => _shipSprite;
    public Color DebugColor => _debugColor;
}
