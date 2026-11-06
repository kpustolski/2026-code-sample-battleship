
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameViewModel : IViewModel
{
    public ObservableList<Point> OccupiedPointList {get; set;} = new();
    public ObservableList<ShipDto> ShipDtoList {get; set;} = new();

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

    public ShipHealthViewModel CreateShipHealthViewModel(Id<Ship> shipId, int startingHealth)
    {
        ShipHealthViewModel newViewModel = new ShipHealthViewModel(shipId, startingHealth, _gameManager);
        return newViewModel;
    }

    public void SetOccupiedPointList(List<Point> occupiedPointList)
    {
        OccupiedPointList.SetAsMutable(occupiedPointList);
    }

    public void SetShipHealthList(List<ShipDto> shipHealth)
    {
        ShipDtoList.SetAsMutable(shipHealth);
    }

    public void UpdateShipHealth(ShipDto shipHealth)
    {
        var ship = ShipDtoList.Value.FirstOrDefault(health => health.ShipId == shipHealth.ShipId);
        if (ship != null)
        {
            ShipDtoList.ReplaceElement(shipHealth);
        }
        else
        {
            this.LogWarning($"ShipHealthList does not contain ship id {shipHealth.ShipId}");
        }
    }

    public void Reset(Id<GameMode> gameModeId)
    {
        _gameModeId = gameModeId;
    }
}