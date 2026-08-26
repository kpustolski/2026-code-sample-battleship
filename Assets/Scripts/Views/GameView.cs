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
    #endregion

    public void SetViewModel(GameViewModel viewModel)
    {
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

        viewModel.Map.DidChange += OnMapChange;
    }

    private void UnSubscribeToViewModel(GameViewModel viewModel)
    {
        if (viewModel == null)
            return;

        viewModel.Map.DidChange -= OnMapChange;
    }

    private void OnMapChange(Map _, Map newValue)
    {
        // Reset the Map if it exists
        MapViewModel mapViewModel = new MapViewModel(newValue);

        if (_currentMapView == null)
        {
            _currentMapView = Instantiate(_mapViewPrefab, _mapViewParentTransform);   
        }

        _currentMapView.SetViewModel(mapViewModel);
    }
}