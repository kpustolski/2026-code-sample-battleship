using System.Linq;
using System.Text;
using UnityEngine;

public partial class GameManager
{
    #region Debug Functions
    public Color DebugGetShipColorByPoint(Point point)
    {
        var shipLocation = GetShipLocation(point);

       return _currentGameModeRenderDef.GetShipRenderDefById(shipLocation.Key).DebugColor;
    }
    
    private void DebugPrintShipLocations()
    {
        StringBuilder sb = new StringBuilder();
        Color color = default;
        string htmlColor = default;
        
        foreach(var shipLoc in _shipLocations)
        {
            color = _currentGameModeRenderDef.GetShipRenderDefById(shipLoc.Key).DebugColor;
            htmlColor = $"#{ColorUtility.ToHtmlStringRGBA(color)}";

            sb.Append($"{shipLoc.Key} | <color={htmlColor}>Color</color> | Points: [");

            foreach(Point point in shipLoc.Value)
            {
                sb.Append($" {point} ");
            }
            sb.Append($"] | Count: {shipLoc.Value.Count}");
            sb.AppendLine();
        }
        this.Log(sb.ToString());
    }
    #endregion
}