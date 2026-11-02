using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Render def scriptable object to hold objects having to do with a game mode.
/// </summary>
[CreateAssetMenu(fileName = "GameModeRenderDef", menuName = "Scriptable Objects/GameModeRenderDef")]
public class GameModeRenderDef : ScriptableObject
{
    [SerializeField]
    private Id<GameMode> _gameModeId = GameModeIds.Default;
    [SerializeField]
    private ShipRenderDefList _shipRenderDefList;

    public Id<GameMode> GameModeId => _gameModeId;
    public ShipRenderDefList ShipRenderDefList => _shipRenderDefList;

    public ShipRenderDef GetShipRenderDefById(Id<Ship> shipId)
    {
        if (_shipRenderDefList == null)
        {
            this.LogError("_shipRenderDefList is null! Unable to get ShipRenderDef.");
            return null;
        }

        return _shipRenderDefList.GetShipRenderDefById(shipId);   
    }
}
