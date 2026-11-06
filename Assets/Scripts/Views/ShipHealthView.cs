using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShipHealthView : MonoBehaviour, IViewModelReceiver<ShipHealthViewModel>
{
    #region Unity References
    [Header("Asset References")]
    [SerializeField]
    private TextMeshProUGUI _debugText;
    #endregion

    #region Private Variables
    private ShipHealthViewModel _viewModel;
    private List<IDisposable> _viewModelSubscriptionList = new List<IDisposable>();
    #endregion

    #region IViewModelReceiver Functions
    public void SetViewModel(ShipHealthViewModel viewModel)
    {
        UnSubscribeToViewModel(_viewModel);
        _viewModel = viewModel;
        SubscribeToViewModel(_viewModel);
    }
    #endregion

    #region Subscription Functions
    private void SubscribeToViewModel(ShipHealthViewModel viewModel)
    {
        if (viewModel == null)
        {
            return;   
        }
        
        var healthSubscription = viewModel.Health.Subscribe(OnHealthChange);
        _viewModelSubscriptionList.Add(healthSubscription);
    }

    private void UnSubscribeToViewModel(ShipHealthViewModel viewModel)
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
    private void OnHealthChange(int _, int newValue)
    {
        if (_debugText == null)
        {
            return;
        }

        //! Temp
        _debugText.text = $"{_viewModel.ShipId}: {_viewModel.Health.Value} / {_viewModel.StartingHealth}";
    }
    #endregion
}