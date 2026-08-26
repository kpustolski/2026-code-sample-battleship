using System.Data;
using UnityEngine;

public class MapView : MonoBehaviour, IViewModelReceiver<MapViewModel>
{
    private MapViewModel _mapViewModel;
    private int _totalRows;
    private int _totalColumns;

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

        viewModel.Map.DidChange += OnMapChange;
    }

    private void UnSubscribeToViewModel(MapViewModel viewModel)
    {
        if (viewModel == null)
            return;
        
        viewModel.Map.DidChange -= OnMapChange;
    }

    private void OnMapChange(Map _, Map newValue)
    {
        _totalRows = newValue.TotalRows;
        _totalColumns = newValue.TotalColumns;

        this.Log($"rows: {_totalRows}, columns: {_totalColumns}");
    }
}