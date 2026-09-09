using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileView : MonoBehaviour, IViewModelReceiver<TileViewModel>
{
    #region Unity References
    [Header("Asset References")]
    [SerializeField]
    private Image _imageComponent;
    #endregion

    private TileViewModel _tileViewModel;
    private List<IDisposable> _viewModelSubscriptionList = new List<IDisposable>();
    
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

        var colorSubscription = viewModel.Color.Subscribe(OnColorChange);
        _viewModelSubscriptionList.Add(colorSubscription);
    }

    private void UnSubscribeToViewModel(TileViewModel viewModel)
    {
        if (viewModel == null)
            return;
    
        // Make sure to remove the callback from the DidChange action.
        foreach (var subscription in _viewModelSubscriptionList)
        {
            subscription.Dispose();
        }
    }

    private void OnColorChange(Color _, Color newValue)
    {
        _imageComponent.color = newValue;
    }
}