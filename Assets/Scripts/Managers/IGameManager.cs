using System.Collections.Generic;

public interface IGameManager
{
    // Functions
    public ShipRenderDef GetShipRenderDefByPoint(Point point);
    public bool CheckForShipHit(Point point);

    // Properties
    public List<Point> PointMap {get;}
    public List<ShipDto> StartingShipDtoList {get;}
}