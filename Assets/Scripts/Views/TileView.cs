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
        var shipSpriteSubscription = viewModel.Sprite.Subscribe(OnShipSpriteChange);
        var isButtonInteractable = viewModel.IsButtonInteractable.Subscribe(OnIsButtonInteractableChange);
        var coverCanvasGroupAlpha = viewModel.CoverCanvasGroupAlpha.Subscribe(OnCoverCanvasGroupAlphaChange);


        _viewModelSubscriptionList.Add(colorSubscription);
        _viewModelSubscriptionList.Add(shipSpriteSubscription);
        _viewModelSubscriptionList.Add(isButtonInteractable);
        _viewModelSubscriptionList.Add(coverCanvasGroupAlpha);
    }

    private void UnSubscribeToViewModel(TileViewModel viewModel)
    {
        if (viewModel == null)
            return;
    
        // Make sure to remove the callback from the DidChange actions.
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
        _shipImageComponent.sprite = newValue;

        if (newValue == null)
        {
            // If there is no sprite, then it's an empty space.
            // Color the space 'blue' for now.
            _shipImageComponent.color = Color.blue;
        }
    }

    private void OnIsButtonInteractableChange(bool _, bool newValue)
    {
        if (_buttonComponent == null)
        {
            return;
        }

        _buttonComponent.interactable = newValue;
    }

    private void OnCoverCanvasGroupAlphaChange(float _, float newValue)
    {
        if (_coverCanvasGroupComponent == null)
        {
            return;
        }

        _coverCanvasGroupComponent.alpha = newValue;
    }

    private void OnTileClicked()
    {
        // Uncover the tile
        _tileViewModel.SetCoverImageAlpha(0.0f);
        // Make sure the button is disabled so the player cannot select it again.
        _tileViewModel.SetIsButtonInteractable(false);
        // Check to see if a ship was hit.
        bool shipHit = _tileViewModel.CheckForShipHit();

        if (shipHit)
        {
            // TODO: Do something fancy.
        }
    }
    #endregion
}