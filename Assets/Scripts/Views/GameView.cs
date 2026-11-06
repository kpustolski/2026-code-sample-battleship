using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
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

    //TODO Better way to store views and their view models?
    private List<TileView> _tileViewList = new List<TileView>();
    private List<TileViewModel> _tileViewModelList = new List<TileViewModel>();

    private List<ShipHealthView> _shipHealthViewList = new List<ShipHealthView>();
    private List<ShipHealthViewModel> _shipHealthViewModelList = new List<ShipHealthViewModel>();

    // TODO: Put this in IViewModelReceiver?
    private List<IDisposable> _viewModelSubscriptionList = new List<IDisposable>();
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

        var occupiedPointListSubscription = viewModel.OccupiedPointList.Subscribe(OnOccupiedPointListChange);
        var shipDtoListSubscription = viewModel.ShipDtoList.Subscribe(OnShipDtoListChange);
        var totalAttemptsLeftSubscription = viewModel.TotalAttemptsLeft.Subscribe(OnTotalAttemptsLeftChange);

        _viewModelSubscriptionList.Add(occupiedPointListSubscription);
        _viewModelSubscriptionList.Add(shipDtoListSubscription);
        _viewModelSubscriptionList.Add(totalAttemptsLeftSubscription);
    }

    private void UnSubscribeToViewModel(GameViewModel viewModel)
    {
        if (viewModel == null)
        {
            return;   
        }
    
        // Make sure to remove the callback from the DidChange action.
        foreach (var subscription in _viewModelSubscriptionList)
        {
            subscription.Dispose();
        }
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
                TileViewModel tileViewModel = _tileViewModelList.FirstOrDefault(viewModel => viewModel.Point.Equals(occupiedPoint));
                if (tileViewModel != default)
                {
                    var renderDef = _gameViewModel.GetShipRenderDefByPoint(occupiedPoint);
                    tileViewModel?.SetRenderDef(renderDef);
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
                var viewModel = _shipHealthViewModelList.FirstOrDefault(viewModel => viewModel.ShipId == ship.ShipId);
                if (viewModel != null)
                {
                    viewModel.SetHealth(ship.Health);
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
        this.Log($"newValue: {newValue}");
        if (_totalAttemptsLeftText == null)
        {
            return;
        }

        _totalAttemptsLeftText.text = $"Total Attempts Left: {newValue}";
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
                _tileViewModelList.Add(viewModel);
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
                ShipHealthViewModel viewModel = _gameViewModel.CreateShipHealthViewModel(ship.ShipId, ship.Health);
                ShipHealthView newView = CreateShipHealthView(ship.ShipId);
                newView.SetViewModel(viewModel);

                _shipHealthViewList.Add(newView);
                _shipHealthViewModelList.Add(viewModel);      
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
        newView.gameObject.name = $"Tile_{point}";

        return newView;
    }

    private ShipHealthView CreateShipHealthView(Id<Ship> shipId)
    {
        if (_shipHealthViewPrefab == null)
        {
            throw new Exception("_shipHealthViewPrefab is null!");
        }

        ShipHealthView newView = Instantiate(_shipHealthViewPrefab, _shipHealthViewParentTransform);
        newView.gameObject.name = $"HealthView_{shipId}";

        return newView;
    }
    #endregion

    #region Reset Functions
    private void ResetTileMap()
    {
        foreach (var viewModel in _tileViewModelList)
        {
            viewModel.Reset();
        }
    }

    private void ResetShipHealthViews()
    {
        foreach (var viewModel in _shipHealthViewModelList)
        {
            viewModel.Reset();
        }
    }
    #endregion
}