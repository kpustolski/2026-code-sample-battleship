using System.Collections.Generic;
using UnityEngine;

public interface IGameManager
{

    // Functions
    public ShipRenderDef GetShipRenderDefByPoint(Point point);
    public bool CheckForShipHit(Point hitPoint);

    // Properties
    public List<Point> PointMap {get;}
}