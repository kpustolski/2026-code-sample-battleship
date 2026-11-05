
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameViewModel : IViewModel
{
    public ObservableList<Point> OccupiedPointList {get; set;} = new();

    public Id<GameMode> GameModeId => _gameModeId;
    public List<Point> PointMap => _gameManager.PointMap;

    private IGameManager _gameManager;
    private Id<GameMode> _gameModeId;

    public GameViewModel(IGameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public ShipRenderDef GetShipRenderDefByPoint(Point point)
    {
        return _gameManager.GetShipRenderDefByPoint(point);
    }

    // We have this helper here so it's easier to pass in the game manager
    // onto new TileViewModels.
    public TileViewModel CreateTileViewModel(Point point)
    {
        TileViewModel newViewModel = new TileViewModel(point, _gameManager);
        return newViewModel;
    }

    public void SetOccupiedPointList(List<Point> occupiedPointList)
    {
        OccupiedPointList.SetAsMutable(occupiedPointList);
    }

    public void Reset(Id<GameMode> gameModeId)
    {
        _gameModeId = gameModeId;
    }
}