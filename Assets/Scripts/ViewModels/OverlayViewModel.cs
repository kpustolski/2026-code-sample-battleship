using System;

public class OverlayViewModel : IViewModel
{
    #region Observable Properties
    public ObservableProperty<string> TitleTextString = new();
    public ObservableProperty<string> ActionButtonTextString = new();
    public ObservableProperty<bool> ShowOverlayView = new();
    #endregion

    #region Properties
    public Action ActionButtonCallback {get; private set;}
    #endregion

    #region Private Variables
    private IGameManager _gameManager;
    #endregion
    
    #region Public Functions
    public OverlayViewModel(IGameManager gameManager, string titleTextString, string actionButtonTextString, bool showView, Action actionButtonCallback = null)
    {
        _gameManager = gameManager;
        ActionButtonCallback = actionButtonCallback;

        SetTitleTextString(titleTextString);
        SetShowOverlayView(showView);

        ActionButtonTextString.SetAsMutable(actionButtonTextString);
    }

    public void SetTitleTextString(string titleTextString)
    {
        TitleTextString.SetAsMutable(titleTextString);
    }

    public void SetShowOverlayView(bool showView)
    {
        ShowOverlayView.SetAsMutable(showView);
    }
    #endregion
}