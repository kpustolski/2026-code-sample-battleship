using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ShipRenderDefList", menuName = "Scriptable Objects/ShipRenderDefList")]
public class ShipRenderDefList : ScriptableObject
{
    [SerializeField]
    private List<ShipRenderDef> ShipRenderDefs = new List<ShipRenderDef>();

    public ShipRenderDef GetShipRenderDefById(Id<Ship> shipId)
    {
        var renderDef = ShipRenderDefs.FirstOrDefault(def => def.ShipId == shipId);

        // If the render def isn't found, then default to the first one in the list.
        // This is under the assumption that the list always has at least one render def
        // in it.
        if (renderDef == null)
        {
            this.LogWarning($"Unable to find ship render def with id {shipId}. Using default render def instead.");
            return ShipRenderDefs[0];   
        }
            
        return ShipRenderDefs.FirstOrDefault(def => def.ShipId == shipId);
    }
}