public class ShipHealthViewModel : IViewModel
{
    #region Observable Properties
    public ObservableProperty<int> Health = new();
    #endregion

    #region Properties
    public Id<Ship> ShipId {get; private set;}
    public string ShipName {get; private set;}
    public int StartingHealth {get; private set;}
    #endregion

    #region Public Functions
    public ShipHealthViewModel(Id<Ship> shipId, string shipName, int startingHealth)
    {
        ShipId = shipId;
        ShipName = shipName;
        StartingHealth = startingHealth;
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