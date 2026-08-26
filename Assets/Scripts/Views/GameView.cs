using System;
using System.Collections.Generic;
using UnityEngine;

public class GameView : MonoBehaviour, IViewModelReceiver<GameViewModel>
{
    #region Unity References
    [Header("Asset References")]
    [SerializeField]
    private Transform _mapViewParentTransform;

    [Header("Prefabs")]
    [SerializeField]
    private MapView _mapViewPrefab;
    #endregion

    #region Variables
    private GameViewModel _gameViewModel;
    private MapView _currentMapView;
    private List<IDisposable> _viewModelSubscriptionList = new List<IDisposable>();
    #endregion

    public void SetViewModel(GameViewModel viewModel)
    {
        this.Log("SetViewModel | Start");
        UnSubscribeToViewModel(_gameViewModel);
        _gameViewModel = viewModel;
        SubscribeToViewModel(_gameViewModel);
    }

    public void Reset()
    {
        // TODO: What happens when the game view is reset?
    }
    
    private void SubscribeToViewModel(GameViewModel viewModel)
    {
        if (viewModel == null)
            return;

        var mapSub = viewModel.Map.Subscribe(OnMapChange);
        _viewModelSubscriptionList.Add(mapSub);
    }

    private void UnSubscribeToViewModel(GameViewModel viewModel)
    {
        if (viewModel == null)
            return;

        // viewModel.Map.DidChange -= OnMapChange;
        
        // Make sure to remove the callback from the DidChange action.
        foreach (var subscription in _viewModelSubscriptionList)
        {
            subscription.Dispose();
        }
    }

    private void OnMapChange(Map _, Map newValue)
    {
        // Reset the Map if it exists
        MapViewModel mapViewModel = new MapViewModel(newValue);

        if (_currentMapView == null)
        {
            this.Log("Creating MapView");
            _currentMapView = Instantiate(_mapViewPrefab, _mapViewParentTransform);   
        }

        _currentMapView.SetViewModel(mapViewModel);
        this.Log($"{mapViewModel}");
    }
}