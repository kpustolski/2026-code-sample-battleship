using NUnit.Framework;

public class DotViewModel : IViewModel
{
    #region Observable Properties
    /// <summary>
    /// If true, the dot view game object is active in the object hierarchy.
    /// </summary>
    public ObservableProperty<bool> IsGameObjectActive = new();
    /// <summary>
    /// If true, the dot view is in it's 'on' state.
    /// Otherwise it's in it's 'off' state.
    /// </summary>
    public ObservableProperty<bool> IsToggleOn = new();
    #endregion

    #region Public functions
    public DotViewModel(bool isGameObjectActive, bool IsToggleOn)
    {
        SetIsGameObjectActive(isGameObjectActive);
    }

    public void SetIsGameObjectActive(bool isGameObjectActive)
    {
        IsGameObjectActive.SetAsMutable(isGameObjectActive);
    }

    public void SetIsIsToggleOn(bool isOn)
    {
        IsToggleOn.SetAsMutable(isOn);
    }
    #endregion
}