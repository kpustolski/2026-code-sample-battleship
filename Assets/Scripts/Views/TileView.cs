using System;
using System.Drawing;
using UnityEngine;

public class TileView : MonoBehaviour, IViewModelReceiver<TileViewModel>
{
    private TileViewModel _tileViewModel;

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
    }

    private void UnSubscribeToViewModel(TileViewModel viewModel)
    {
        if (viewModel == null)
            return;
    }
}