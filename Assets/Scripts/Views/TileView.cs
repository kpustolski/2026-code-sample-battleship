using System;
using System.Drawing;
using UnityEngine;

public class TileView : MonoBehaviour, IViewModelReceiver<TileViewModel>
{
    private TileViewModel _tileViewModel;
    private Vector2 _worldPosition;

    public void SetViewModel(TileViewModel viewModel)
    {
        UnSubscribeToViewModel(_tileViewModel);
        _tileViewModel = viewModel;
        SubscribeToViewModel(_tileViewModel);
    }

    private void SubscribeToViewModel(TileViewModel viewModel)
    {
        if (viewModel == null)
            return;
        
        viewModel.WorldPositionX.DidChange += OnWorldPositionXDidChange;
        viewModel.WorldPositionY.DidChange += OnWorldPositionYDidChange;
    }

    private void UnSubscribeToViewModel(TileViewModel viewModel)
    {
        if (viewModel == null)
            return;
        
        viewModel.WorldPositionX.DidChange -= OnWorldPositionXDidChange;
        viewModel.WorldPositionY.DidChange -= OnWorldPositionYDidChange;
    }

    private void OnWorldPositionXDidChange(float oldValue, float newValue)
    {
        if (_worldPosition == null)
        {
            _worldPosition = new Vector2(newValue, 0);
        }
        else
        {
            float currentYPos = _worldPosition.y;
            _worldPosition = new Vector2(newValue, currentYPos);
        }

        this.Log($"oldValue: {oldValue} | newValue: {newValue}");
    }
    
    private void OnWorldPositionYDidChange(float oldValue, float newValue)
    {
        if (_worldPosition == null)
        {
            _worldPosition = new Vector2(0, newValue);
        }
        else
        {
            float currentXPos = _worldPosition.x;
            _worldPosition = new Vector2(currentXPos, newValue);
        }

        this.Log($"oldValue: {oldValue} | newValue: {newValue}");
    }
}