using System.Linq;
using System.Text;
using UnityEngine;

public partial class GameManager
{
    #region Debug Functions
    public Color DebugGetShipColorByPoint(Point point)
    {
        var shipLocation = _shipLocations.FirstOrDefault((shipLoc =>
        {
            Point p = shipLoc.Value.FirstOrDefault(p => p == point);
            return p != default;
        }));

       return DebugGetShipColorById(shipLocation.Key);
    }

    private Color DebugGetShipColorById(Id<Ship> shipId)
    {
        var colorRGB = ShipDefList.GetDefById(shipId).DebugColor;
        Color color = new Color(colorRGB.r, colorRGB.g, colorRGB.b);
        return color;
    }
    
    private void DebugPrintShipLocations()
    {
        StringBuilder sb = new StringBuilder();
        Color color = default;
        string htmlColor = default;
        
        foreach(var shipLoc in _shipLocations)
        {
            color = DebugGetShipColorById(shipLoc.Key);
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