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
        
        viewModel.Health.Subscribe(OnHealthChange);
    }

    private void UnSubscribeToViewModel(ShipHealthViewModel viewModel)
    {
        if (viewModel == null)
        {
            return;
        }

        viewModel.Health.Unsubscribe(OnHealthChange);
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