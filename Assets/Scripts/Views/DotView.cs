using UnityEngine;
using UnityEngine.UI;

public class DotView : MonoBehaviour, IViewModelReceiver<DotViewModel>
{
    #region Asset References
    [SerializeField]
    private Toggle _toggleScript;
    #endregion

    #region Private Variables
    private DotViewModel _dotViewModel;
    #endregion

    #region Public Variables
    public bool IsToggleOn => _toggleScript.isOn;
    public DotViewModel ViewModel => _dotViewModel;
    #endregion

    #region Unity Overrides
    public void OnDestroy()
    {
        UnsubscribeToViewModel(_dotViewModel);
    }
    #endregion

    #region IViewModelReceiver Functions
    public void SetViewModel(DotViewModel viewModel)
    {
        UnsubscribeToViewModel(_dotViewModel);
        _dotViewModel = viewModel;
        SubscribeToViewModel(_dotViewModel);
    }
    #endregion

    #region Subscription Functions
    private void SubscribeToViewModel(DotViewModel viewModel)
    {
        if (viewModel == null)
        {
            return;
        }

        viewModel.IsGameObjectActive.DidChange += OnIsGameObjectActiveChanged;
        viewModel.IsToggleOn.DidChange += OnIsToggleOn;
    }

    private void UnsubscribeToViewModel(DotViewModel viewModel)
    {
        if (viewModel == null)
        {
            return;
        }

        viewModel.IsGameObjectActive.DidChange -= OnIsGameObjectActiveChanged;
        viewModel.IsToggleOn.DidChange -= OnIsToggleOn;
    }
    #endregion

    #region Subscription Callbacks
    private void OnIsGameObjectActiveChanged(bool _, bool newValue)
    {
        this.gameObject.SetActive(newValue);
    }

    private void OnIsToggleOn(bool _, bool newValue)
    {
        if (_toggleScript == null)
        {
            return;
        }

        _toggleScript.isOn = newValue;
    }
    #endregion
}