
using System.Collections.Generic;
using System.Linq;
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
}