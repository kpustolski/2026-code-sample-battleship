using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TileView : MonoBehaviour, IViewModelReceiver<TileViewModel>
{
    #region Unity References
    [Header("Asset References")]
    [SerializeField]
    private Image _imageComponent;
    [SerializeField]
    private Button _buttonComponent;
    [SerializeField]
    private TextMeshProUGUI _debugPointText;
    #endregion

    #region Private Variables
    private TileViewModel _tileViewModel;
    private List<IDisposable> _viewModelSubscriptionList = new List<IDisposable>();
    #endregion

    #region Unity Overrides
    private void OnEnable()
    {
        if (_buttonComponent != null)
            _buttonComponent.onClick.AddListener(OnTileClicked);
    }

    private void OnDisable()
    {
        if (_buttonComponent != null)
            _buttonComponent.onClick.RemoveListener(OnTileClicked);
    }
    #endregion

    #region Public Functions
    public void SetViewModel(TileViewModel viewModel)
    {
        UnSubscribeToViewModel(_tileViewModel);
        _tileViewModel = viewModel;
        SubscribeToViewModel(_tileViewModel);

        //! Debug
        _debugPointText.text = _tileViewModel.Point.ToString();
        _debugPointText.color = Color.black;
        _debugPointText.fontWeight = FontWeight.Bold;
    }
    #endregion

    #region Private Functions
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

    private void OnTileClicked()
    {
        this.Log($"Point clicked: {_tileViewModel.Point}");
    }
    #endregion
}