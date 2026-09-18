using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameView : MonoBehaviour, IViewModelReceiver<GameViewModel>
{
    #region Unity References
    [Header("Asset References")]
    [SerializeField]
    private Transform _tileViewParentTransform;

    [Header("Prefabs")]
    [SerializeField]
    private TileView _tileViewPrefab;
    #endregion

    #region Variables
    private GameViewModel _gameViewModel;

    //TODO Better way to store views and their view models?
    private List<TileView> _tileViewList = new List<TileView>();
    private List<TileViewModel> _tileViewModelList = new List<TileViewModel>();

    // TODO: Put this in IViewModelReceiver?
    private List<IDisposable> _viewModelSubscriptionList = new List<IDisposable>();
    #endregion

    public void SetViewModel(GameViewModel viewModel)
    {
        UnSubscribeToViewModel(_gameViewModel);
        _gameViewModel = viewModel;
        
        CreateTileMap();
        SubscribeToViewModel(_gameViewModel);
    }

    private void SubscribeToViewModel(GameViewModel viewModel)
    {
        if (viewModel == null)
            return;

        var occupiedPointListSubscription = viewModel.OccupiedPointList.Subscribe(OnOccupiedPointListChange);
        _viewModelSubscriptionList.Add(occupiedPointListSubscription);
    }

    private void UnSubscribeToViewModel(GameViewModel viewModel)
    {
        if (viewModel == null)
            return;
    
        // Make sure to remove the callback from the DidChange action.
        foreach (var subscription in _viewModelSubscriptionList)
        {
            subscription.Dispose();
        }
    }

    private void OnOccupiedPointListChange(List<Point> _, List<Point> newValue)
    {
        Color color = default;
        // Reset the Map if it exists
        // TODO: Test try catch loop
        try
        {
            // Reset tile views
            ResetTileMap();

            foreach (Point occupiedPoint in newValue)
            {
                // Find the corresponding view model and set the tile color to red.
                TileViewModel tileViewModel = _tileViewModelList.FirstOrDefault(viewModel => viewModel.Point.Equals(occupiedPoint));
                if (tileViewModel != default)
                {
                    color = _gameViewModel.DebugGetShipColorByPoint(occupiedPoint);
                    tileViewModel?.SetColor(color);
                }
            }
        }
        catch (Exception e)
        {
            this.LogError($"Unable to create a map view. Error: {e}");
        }
    }
    private void CreateTileMap()
    {
        // If tile views don't exist yet, create a new series of them.
        if (_tileViewList.Count == 0)
        {
            foreach (var point in _gameViewModel.PointMap)
            {
                TileViewModel newViewModel = new TileViewModel(point);
                TileView newView = CreateTileView(point);
                newView.SetViewModel(newViewModel);

                _tileViewList.Add(newView);
                _tileViewModelList.Add(newViewModel);
            }
        }
    }

    private void ResetTileMap()
    {
        // For now, change the color of the tiles back to white
        foreach (var viewModel in _tileViewModelList)
        {
            viewModel.SetColor(Color.white);
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
}