using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameView : MonoBehaviour, IViewModelReceiver<GameViewModel>
{
    #region Unity References
    [Header("Asset References")]
    [SerializeField]
    private Transform _tileViewParentTransform;
    [SerializeField]
    private Transform _shipHealthViewParentTransform;
    [SerializeField]
    private TextMeshProUGUI _totalAttemptsLeftText;

    [Header("Prefabs")]
    [SerializeField]
    private TileView _tileViewPrefab;
    [SerializeField]
    private ShipHealthView _shipHealthViewPrefab;
    #endregion

    #region Private Variables
    private GameViewModel _gameViewModel;

    private List<TileView> _tileViewList = new List<TileView>();
    private List<ShipHealthView> _shipHealthViewList = new List<ShipHealthView>();
    #endregion

    #region IViewModelReceiver Functions
    public void SetViewModel(GameViewModel viewModel)
    {
        UnSubscribeToViewModel(_gameViewModel);
        _gameViewModel = viewModel;
        
        CreateTileMap();
        CreateShipHealthViews();
        SubscribeToViewModel(_gameViewModel);
    }
    #endregion

    #region Subscription Functions
    private void SubscribeToViewModel(GameViewModel viewModel)
    {
        if (viewModel == null)
        {
            return;   
        }

        viewModel.OccupiedPointList.Subscribe(OnOccupiedPointListChange);
        viewModel.ShipDtoList.Subscribe(OnShipDtoListChange);
        viewModel.TotalAttemptsLeft.Subscribe(OnTotalAttemptsLeftChange);
    }

    private void UnSubscribeToViewModel(GameViewModel viewModel)
    {
        if (viewModel == null)
        {
            return;   
        }
    
        viewModel.OccupiedPointList.Unsubscribe(OnOccupiedPointListChange);
        viewModel.ShipDtoList.Unsubscribe(OnShipDtoListChange);
        viewModel.TotalAttemptsLeft.Unsubscribe(OnTotalAttemptsLeftChange);
    }
    #endregion
    
    #region Subscription Callbacks
    private void OnOccupiedPointListChange(List<Point> _, List<Point> newValue)
    {
        // Reset the Map if it exists
        // TODO: Test try catch loop
        try
        {
            // Reset tile views
            ResetTileMap();
            // The occupied point list changes when we reset the game.
            // So we'll want to also reset the health views as well.
            ResetShipHealthViews();

            foreach (Point occupiedPoint in newValue)
            {
                TileView tileView = _tileViewList.FirstOrDefault(view => view.ViewModel.Point.Equals(occupiedPoint));
                if (tileView != default)
                {
                    var renderDef = _gameViewModel.GetShipRenderDefByPoint(occupiedPoint);
                    tileView.ViewModel.SetRenderDef(renderDef);
                }
            }
        }
        catch (Exception e)
        {
            this.LogError($"Unable to create a map view. Error: {e}");
        }
    }

    private void OnShipDtoListChange(List<ShipDto> _, List<ShipDto> newValue)
    {
        try
        {
            foreach (ShipDto ship in newValue)
            {
                ShipHealthView view = _shipHealthViewList.FirstOrDefault(view => view.ViewModel.ShipId == ship.ShipId);
                if (view != null)
                {
                    view.ViewModel.SetHealth(ship.Health);
                }
                else
                {
                    this.LogWarning($"Unable to find ShipHealthViewModel for ship with id {ship.ShipId}.");
                }
            }
        }
        catch(Exception e)
        {
            this.LogError($"Unable to create ship health views. Error: {e}");
        }
    }

    private void OnTotalAttemptsLeftChange(int _, int newValue)
    {
        // In a more complex project, there would be a whole localization system that
        // would map to the appropriate text. However, since this project's scope
        // does not include localization, I'm setting the text directly in code.
        string attemptsLeftString = $"Total Attempts Left: {newValue}";

        if (_totalAttemptsLeftText == null)
        {
            return;
        }

        // Change the color of the number string to red once the color passes a specific threshold.
        if (newValue <= Constants.AttemptsLeftColorThreshold)
        {
            attemptsLeftString = $"Total Attempts Left: <color=\"red\">{newValue}</color>";
        }

        _totalAttemptsLeftText.text = attemptsLeftString;
    }
    #endregion

    #region Creation Functions
    private void CreateTileMap()
    {
        // If tile views don't exist yet, create a new series of them.
        if (_tileViewList.Count == 0)
        {
            foreach (var point in _gameViewModel.PointMap)
            {
                TileViewModel viewModel = _gameViewModel.CreateTileViewModel(point);
                TileView newView = CreateTileView(point);
                newView.SetViewModel(viewModel);

                _tileViewList.Add(newView);
            }
        }
    }

    private void CreateShipHealthViews()
    {
        // If ship health views don't exist yet, create a new series of them.
        if (_shipHealthViewList.Count == 0)
        {
            foreach (var ship in _gameViewModel.StartingShipDtoList)
            {
                ShipRenderDef renderDef = _gameViewModel.GetShipRenderDefById(ship.ShipId);
                ShipHealthViewModel viewModel = _gameViewModel.CreateShipHealthViewModel(ship.ShipId, ship.ShipName, ship.Health, renderDef);
                ShipHealthView newView = CreateShipHealthView(ship.ShipId);
                newView.SetViewModel(viewModel);

                _shipHealthViewList.Add(newView);
            }
        }
    }
    
    private TileView CreateTileView(Point point)
    {
        if (_tileViewPrefab == null)
        {
            throw new Exception("_tileViewPrefab is null!");
        }

        TileView newView = Instantiate(_tileViewPrefab, _tileViewParentTransform);
        newView.gameObject.name = $"{nameof(TileView)}_{point}";

        return newView;
    }

    private ShipHealthView CreateShipHealthView(Id<Ship> shipId)
    {
        if (_shipHealthViewPrefab == null)
        {
            throw new Exception("_shipHealthViewPrefab is null!");
        }

        ShipHealthView newView = Instantiate(_shipHealthViewPrefab, _shipHealthViewParentTransform);
        newView.gameObject.name = $"{nameof(ShipHealthView)}_{shipId}";

        return newView;
    }
    #endregion

    #region Reset Functions
    private void ResetTileMap()
    {
        foreach (var viewModel in _tileViewList)
        {
            viewModel.ViewModel.Reset();
        }
    }

    private void ResetShipHealthViews()
    {
        foreach (var view in _shipHealthViewList)
        {
            view.ViewModel.Reset();
        }
    }
    #endregion
}