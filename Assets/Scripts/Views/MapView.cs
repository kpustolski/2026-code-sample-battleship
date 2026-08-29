using System;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class MapView : MonoBehaviour, IViewModelReceiver<MapViewModel>
{
    #region Unity References
    [Header("Asset References")]
    [SerializeField]
    private Transform _tileViewParentTransform;

    [Header("Prefabs")]
    [SerializeField]
    private TileView _tileViewPrefab;
    #endregion

    private MapViewModel _mapViewModel;
    private List<TileView> _tileViewList = new List<TileView>();
    private List<IDisposable> _viewModelSubscriptionList = new List<IDisposable>();

    public void SetViewModel(MapViewModel viewModel)
    {
        UnSubscribeToViewModel(_mapViewModel);
        _mapViewModel = viewModel;
        SubscribeToViewModel(_mapViewModel);
    }
    
    private void SubscribeToViewModel(MapViewModel viewModel)
    {
        if (viewModel == null)
            return;
        
        var tileListSub = viewModel.TileList.Subscribe(OnTileListChange);
        _viewModelSubscriptionList.Add(tileListSub);
    }

    private void UnSubscribeToViewModel(MapViewModel viewModel)
    {
        if (viewModel == null)
            return;
        
        // Make sure to remove the callback from the DidChange action.
        foreach (var subscription in _viewModelSubscriptionList)
        {
            subscription.Dispose();
        }
    }

    private void OnTileListChange(List<Tile> _, List<Tile> newValue)
    {
        // TODO: Test try catch loop
        try
        {
            // If tile views don't exist yet, create a new series of them.
            if (_tileViewList.Count == 0)
            {
                foreach (var tile in newValue)
                {
                    TileView newView = CreateTileView(tile);
                    _tileViewList.Add(newView);
                }
            }
            // Otherwise, update the existing map
            else
            {
                //TODO: TBD. May not be necessary
            }
        }
        catch (Exception e)
        {
            this.LogError($"Unable to create a map view. Error: {e}");
        }
    }

    private TileView CreateTileView(Tile tile)
    {
        if (_tileViewPrefab == null)
        {
            throw new Exception("_tileViewPrefab is null!");
        }

        TileViewModel newViewModel = new TileViewModel(tile);
        TileView newView = Instantiate(_tileViewPrefab, _tileViewParentTransform);
        newView.gameObject.name = $"Tile_{newViewModel.Tile.Value.Point}";
        newView.SetViewModel(newViewModel);

        return newView;
    }
}