using System.Collections.Generic;
using UnityEngine;

public interface IGameManager
{

    // Functions
    public ShipRenderDef GetShipRenderDefByPoint(Point point);

    //!  Debug Functions
    public Color DebugGetShipColorByPoint(Point point);

    // Properties
    public List<Point> PointMap {get;}
}