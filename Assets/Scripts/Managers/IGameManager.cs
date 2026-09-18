using System.Collections.Generic;
using UnityEngine;

public interface IGameManager
{

    // Functions
    public Color DebugGetShipColorByPoint(Point point);

    // Properties
    public List<Point> PointMap {get;}
}