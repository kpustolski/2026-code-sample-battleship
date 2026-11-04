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
    private CanvasGroup _coverCanvasGroupComponent;
    [SerializeField]
    private Image _shipBackgroundComponent;
    [SerializeField]
    private Image _shipImageComponent;
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

        var colorSubscription = viewModel.DebugColor.Subscribe(OnColorChange);
        _viewModelSubscriptionList.Add(colorSubscription);

        var shipSpriteSubscription = viewModel.Sprite.Subscribe(OnShipSpriteChange);
        _viewModelSubscriptionList.Add(shipSpriteSubscription);
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
        if (_shipBackgroundComponent == null)
            return;
        
        //! Temp
        
        _shipBackgroundComponent.color = newValue;
    }

    private void OnShipSpriteChange(Sprite _, Sprite newValue)
    {
        // It is ok if the sprite is null since they can be "empty".
        _shipImageComponent.sprite = newValue;
    }

    private void OnTileClicked()
    {
        if (_coverCanvasGroupComponent == null)
        {
            return;   
        }

        _coverCanvasGroupComponent.alpha = 0;
        
        _tileViewModel.CheckForShipHit();

        //TODO: If there's a ship, we need to track that it's been hit.
        //TODO: If it's not a ship, we need to mark that the empty tile has been hit.
        // TODO: Don't do anything if the tile was already revealed.
    }
    #endregion
}