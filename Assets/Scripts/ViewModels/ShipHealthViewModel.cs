public class ShipHealthViewModel : IViewModel
{
    #region Observable Properties
    public ObservableProperty<int> Health = new();
    #endregion

    #region Properties
    public Id<Ship> ShipId => _shipId;
    public int StartingHealth => _startingHealth;
    #endregion

    #region Private Variables
    private Id<Ship> _shipId;
    private int _startingHealth;
    #endregion 

    #region Public Functions
    public ShipHealthViewModel(Id<Ship> shipId, int startingHealth)
    {
        _shipId = shipId;
        _startingHealth = startingHealth;
    }

    public void SetHealth(int health)
    {
        Health.SetAsMutable(health);
    }

    public void Reset()
    {
        // TODO: When this view is more complex, what would be it's default state?
    }
    #endregion
}