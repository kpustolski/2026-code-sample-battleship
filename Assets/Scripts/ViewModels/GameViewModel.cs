
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameViewModel : IViewModel
{
    #region Observable Properties
    public ObservableList<Point> OccupiedPointList {get; set;} = new();
    public ObservableList<ShipDto> ShipDtoList {get; set;} = new();
    #endregion

    #region Properties
    public Id<GameMode> GameModeId => _gameModeId;
    public List<Point> PointMap => _gameManager.PointMap;
    public List<ShipDto> StartingShipDtoList => _gameManager.StartingShipDtoList;
    #endregion

    #region Observable Properties
    private IGameManager _gameManager;
    private Id<GameMode> _gameModeId;
    #endregion

    #region Public Functions
    public GameViewModel(IGameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public ShipRenderDef GetShipRenderDefByPoint(Point point)
    {
        return _gameManager.GetShipRenderDefByPoint(point);
    }

    /// <summary>
    /// Creates a TileViewModel.
    /// This helper is here so it's easier to pass in the game manager dependency
    /// into new TileViewModels.
    /// </summary>
    public TileViewModel CreateTileViewModel(Point point)
    {
        TileViewModel newViewModel = new TileViewModel(point, _gameManager);
        return newViewModel;
    }

    /// <summary>
    /// Creates a ShipHealthViewModel.
    /// This helper can be placed in GameView.cs, but I wanted to keep the view model
    /// helpers together for organizaitonal purposes.
    /// </summary>
    public ShipHealthViewModel CreateShipHealthViewModel(Id<Ship> shipId, int startingHealth)
    {
        ShipHealthViewModel newViewModel = new ShipHealthViewModel(shipId, startingHealth);
        return newViewModel;
    }

    public void SetOccupiedPointList(List<Point> occupiedPointList)
    {
        OccupiedPointList.SetAsMutable(occupiedPointList);
    }

    public void SetShipDtoList(List<ShipDto> shipDtoList)
    {
        ShipDtoList.SetAsMutable(shipDtoList);
    }

    public void UpdateShipDto(ShipDto shipDto)
    {
        var ship = ShipDtoList.Value.FirstOrDefault(health => health.ShipId == shipDto.ShipId);
        if (ship != null)
        {
            ShipDtoList.ReplaceElement(shipDto);
        }
        else
        {
            this.LogWarning($"ShipHealthList does not contain ship id {shipDto.ShipId}");
        }
    }

    public void Reset(Id<GameMode> gameModeId)
    {
        _gameModeId = gameModeId;
    }
    #endregion
}