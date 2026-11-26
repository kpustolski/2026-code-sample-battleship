using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShipHealthView : MonoBehaviour, IViewModelReceiver<ShipHealthViewModel>
{
    #region Unity References
    [Header("Asset References")]
    [SerializeField]
    private TextMeshProUGUI _shipNameText;
    [SerializeField]
    private Image _shipIconImage;
    [SerializeField]
    private RectTransform _skullIconContainer;
    [SerializeField]
    private RectTransform _dotViewsParentTransform;

    [Header("Prefabs")]
    [SerializeField]
    private DotView _dotViewPrefab;
    #endregion

    #region Private Variables
    private ShipHealthViewModel _shipHealthViewModel;
    private List<DotView> _dotViewsList = new List<DotView>();
    #endregion

    #region Public Variables
    public ShipHealthViewModel ViewModel => _shipHealthViewModel;
    #endregion

    #region IViewModelReceiver Functions
    public void SetViewModel(ShipHealthViewModel viewModel)
    {
        UnSubscribeToViewModel(_shipHealthViewModel);
        _shipHealthViewModel = viewModel;
        SubscribeToViewModel(_shipHealthViewModel);
    }
    #endregion

    #region Subscription Functions
    private void SubscribeToViewModel(ShipHealthViewModel viewModel)
    {
        if (viewModel == null)
        {
            return;   
        }
        
        Reset();
        CreateDotViews();
        // It should be ok to set these once and not have them
        // adhere to the Observable Property setup.
        // They remain the same throughout the game.
        _shipNameText.text = _shipHealthViewModel.ShipName;
        _shipIconImage.sprite = _shipHealthViewModel.RenderDef.ShipSprite;

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
        if (_dotViewsList == null || _dotViewsList.Count == 0)
        {
            return;
        }

        // The UI displays the number of times the player
        // hits a ship instead of it's actual health value. 
        // This means the UI displays the delta
        // between the ship's starting health and current health.
        // For example, if the ship's health is 3/5, that means the ship was hit 2 times.
        // 2 dot views would be toggled on in the UI.
        int healthDelta = _shipHealthViewModel.StartingHealth - newValue;
        if (healthDelta == 0)
        {
            ResetDotViewToggles();
        }
        else
        {
            for (int i = 0; i < healthDelta; i++)
            {
                // In this game, the health of a ship only decreases and never increases.
                // Therefore, we only need to worry about toggling the dots "on" here.
                // If there was a rule where the ships could heal, then he would
                // also need to add logic for toggling the view 'off'.
                _dotViewsList[i].ViewModel.SetIsIsToggleOn(true);
            }   
        }

        // Show the skull icon if the ship is sunk!
        _skullIconContainer.gameObject.SetActive(newValue == 0);


        // Internal functions

        // This will reset the views for when
        // the game is reset.
        void ResetDotViewToggles()
        {
            for (int i = 0; i < _dotViewsList.Count; i++)
            {
                _dotViewsList[i].ViewModel.SetIsIsToggleOn(false);
            }
        }
    }
    #endregion

    #region Helpers
    /// <summary>
    /// Creates the dot views and their view models.
    /// Note: I could implement an object pooling structure for these
    /// dot views, but I think that's a little overkill for this project.
    /// This set up is fine for the scope of this game.
    /// </summary>
    private void CreateDotViews()
    {
        if (_dotViewPrefab == null)
        {
            return;
        }

        if (_dotViewsList.Count == 0)
        {
            for (int i = 0; i < _shipHealthViewModel.StartingHealth; i++)
            {
                DotViewModel viewModel = new DotViewModel(isGameObjectActive: true, IsToggleOn: false);
                DotView newDot = Instantiate(_dotViewPrefab, _dotViewsParentTransform);
                newDot.name = $"{nameof(DotView)}_{i}";
                newDot.SetViewModel(viewModel);

                _dotViewsList.Add(newDot);
            }
        }
    }

    private void Reset()
    {
        // Since the game is rather lightweight,
        // destroying the objects instead of keeping them
        // around is ok here. In a more complex project
        // I'd consider keeping the objects around and
        // controlling their active state instead.
        // That's where an object pooling set up could be useful.
        foreach (var dot in _dotViewsList)
        {
            Destroy(dot.gameObject);
        }

        _dotViewsList.Clear();
    }
    #endregion
}