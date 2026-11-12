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
    
    #region Public Functions
    public OverlayViewModel(string titleTextString, string actionButtonTextString, bool showView, Action actionButtonCallback = null)
    {
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