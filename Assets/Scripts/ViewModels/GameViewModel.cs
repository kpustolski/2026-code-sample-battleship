
using System.Collections.Generic;
using UnityEngine;

public class GameViewModel : IViewModel
{
    public ObservableList<Point> OccupiedPointList {get; set;} = new();
    public Id<GameMode> GameModeId {get; set;}
    public List<Point> PointMap => _gameManager.PointMap;

    private IGameManager _gameManager;

    public GameViewModel(Id<GameMode> gameModeId, List<Point> occupiedPointList, IGameManager gameManager)
    {
        _gameManager = gameManager;

        GameModeId = gameModeId;
        OccupiedPointList.SetAsMutable(occupiedPointList);
    }

    public Color DebugGetShipColorByPoint(Point point)
    {
        return _gameManager.DebugGetShipColorByPoint(point);
    }
}