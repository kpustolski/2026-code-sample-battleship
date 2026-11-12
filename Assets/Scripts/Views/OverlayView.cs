using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OverlayView : MonoBehaviour, IViewModelReceiver<OverlayViewModel>
{
    #region Unity References
    [Header("Asset References")]
    [SerializeField]
    private TextMeshProUGUI _titleText;
    [SerializeField]
    private TextMeshProUGUI _actionButtonText;
    [SerializeField]
    private Button _actionButton;
    #endregion
    
    #region Private Variables
    private OverlayViewModel _overlayViewModel;
    #endregion

    #region IViewModelReceiver Functions
    public void SetViewModel(OverlayViewModel viewModel)
    {
        UnsubscribeToViewModel(_overlayViewModel);
        _overlayViewModel = viewModel;
        SubscribeToViewModel(_overlayViewModel);

        // First unsubscribe button callbacks to avoid any
        // duplicate assignments to the OnClick listener.
        // Then subscribe once more.
        UnsubscribeToButtonCallbacks();
        SubscribeToButtonCallbacks();
    }
    #endregion

    #region Subscription Functions
    private void SubscribeToViewModel(OverlayViewModel viewModel)
    {
        if (viewModel == null)
        {
            return;
        }

        viewModel.ShowOverlayView.Subscribe(OnShowOverlayViewChange);
        viewModel.TitleTextString.Subscribe(OnTitleTextStringChange);
        viewModel.ActionButtonTextString.Subscribe(OnActionButtonTextStringChange);
    }

    private void UnsubscribeToViewModel(OverlayViewModel viewModel)
    {
        if (viewModel == null)
        {
            return;
        }

        viewModel.ShowOverlayView.Unsubscribe(OnShowOverlayViewChange);
        viewModel.TitleTextString.Unsubscribe(OnTitleTextStringChange);
        viewModel.ActionButtonTextString.Unsubscribe(OnActionButtonTextStringChange);
    }

    private void SubscribeToButtonCallbacks()
    {
        if (_actionButton != null)
        {
            _actionButton.onClick.AddListener(OnActionButtonClicked);   
        }
    }

    private void UnsubscribeToButtonCallbacks()
    {
        if (_actionButton != null)
        {
            _actionButton.onClick.RemoveListener(OnActionButtonClicked);   
        }
    }
    #endregion

    #region Subscription Callbacks
    private void OnShowOverlayViewChange(bool _, bool newValue)
    {
        gameObject.SetActive(newValue);
    }

    private void OnTitleTextStringChange(string _, string newValue)
    {
        if (_titleText == null)
        {
            return;
        }

        _titleText.text = newValue;
    }

    private void OnActionButtonTextStringChange(string _, string newValue)
    {
        if (_actionButtonText == null)
        {
            return;
        }

        _actionButtonText.text = newValue;
    }

    private void OnActionButtonClicked()
    {
        _overlayViewModel.ActionButtonCallback.Invoke();
    }
    #endregion
}