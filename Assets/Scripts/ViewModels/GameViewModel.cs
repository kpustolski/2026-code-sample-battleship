
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameViewModel : IViewModel
{
    public ObservableList<Point> OccupiedPointList {get; set;} = new();
    public Id<GameMode> GameModeId {get; set;}

    // TileViewModels should be persistent. So we don't need to make an ObservableList with this.
    public List<TileViewModel> TileViewModels => _tileViewModels;
    public List<Point> PointMap => _gameManager.PointMap;

    private IGameManager _gameManager;
    private List<TileViewModel> _tileViewModels = new List<TileViewModel>();

    public GameViewModel(Id<GameMode> gameModeId, List<Point> occupiedPointList, IGameManager gameManager)
    {
        _gameManager = gameManager;

        GameModeId = gameModeId;
        OccupiedPointList.SetAsMutable(occupiedPointList);
        
        CreateTileViewModels();
    }

    public ShipRenderDef GetShipRenderDefByPoint(Point point)
    {
        return _gameManager.GetShipRenderDefByPoint(point);
    }

    public void CreateTileViewModels()
    {
        foreach (var point in PointMap)
        {
            TileViewModel newViewModel = new TileViewModel(point, _gameManager);
            _tileViewModels.Add(newViewModel);
        }
    }

    public void ResetTileViewModels()
    {
        foreach (var viewModel in _tileViewModels)
        {
            viewModel.Reset();
        }
    }

    public TileViewModel GetTileViewModelByPoint(Point point)
    {
         return _tileViewModels.FirstOrDefault(viewModel => viewModel.Point.Equals(point));
    }
}