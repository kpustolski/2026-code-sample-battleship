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

        viewModel.TotalRows.DidChange += OnTotalRowsDidChange;
        viewModel.TotalColumns.DidChange += OnTotalColumnsDidChange;
    }

    private void UnSubscribeToViewModel(MapViewModel viewModel)
    {
        if (viewModel == null)
            return;
        
        viewModel.TotalRows.DidChange -= OnTotalRowsDidChange;
        viewModel.TotalColumns.DidChange -= OnTotalColumnsDidChange;
    }

    private void OnTotalRowsDidChange(int _, int newValue)
    {
        _totalRows = newValue;
        Debug.Log($"OnTotalRowsDidChange | oldValue: {_} | newValue: {newValue}");

    }

    private void OnTotalColumnsDidChange(int _, int newValue)
    {
        _totalColumns = newValue;
        Debug.Log($"OnTotalColumnsDidChange | oldValue: {_} | newValue: {newValue}");
    }
}