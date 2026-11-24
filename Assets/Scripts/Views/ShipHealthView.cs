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
    //TODO: Make dotView classes
    [SerializeField]
    private GameObject _dotViewPrefab;
    #endregion

    #region Private Variables
    private ShipHealthViewModel _viewModel;
    private List<GameObject> _dotViewsList = new List<GameObject>();
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
        
        Reset();
        CreateDotViews();
        // It should be ok to set these once and not have them
        // adhere to the Observable Property setup.
        // They remain the same throughout the game.
        _shipNameText.text = _viewModel.ShipName;
        _shipIconImage.sprite = _viewModel.RenderDef.ShipSprite;

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
        // if (_debugText == null)
        // {
        //     return;
        // }

        // //! Temp
        // _debugText.text = $"{_viewModel.ShipId}: {_viewModel.Health.Value} / {_viewModel.StartingHealth}";

        // Show the skull icon if the ship is sunk!
        _skullIconContainer.gameObject.SetActive(newValue == 0);
    }
    #endregion

    #region Helpers
    private void CreateDotViews()
    {
        if (_dotViewPrefab == null)
        {
            return;
        }

        if (_dotViewsList.Count == 0)
        {
            for (int i = 0; i < _viewModel.StartingHealth; i++)
            {
                GameObject newDot = Instantiate(_dotViewPrefab, _dotViewsParentTransform);
                _dotViewsList.Add(newDot);
            }
        }
    }

    private void Reset()
    {
        foreach (var dot in _dotViewsList)
        {
            Destroy(dot.gameObject);
        }

        _dotViewsList.Clear();
    }
    #endregion
}