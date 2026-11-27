
public class ShipHealthViewModel : IViewModel
{
    #region Observable Properties
    public ObservableProperty<int> Health = new();
    #endregion

    #region Properties
    public Id<Ship> ShipId {get; private set;}
    public string ShipName {get; private set;}
    public ShipRenderDef RenderDef {get; private set;}
    public int StartingHealth {get; private set;}
    #endregion

    #region Public Functions
    public ShipHealthViewModel(Id<Ship> shipId, string shipName, int startingHealth, ShipRenderDef renderDef)
    {
        ShipId = shipId;
        RenderDef = renderDef;
        ShipName = shipName;
        StartingHealth = startingHealth;

        SetHealth(StartingHealth);
    }

    public void SetHealth(int health)
    {
        Health.SetAsMutable(health);
    }

    public void Reset()
    {
        SetHealth(StartingHealth);
    }
    #endregion
}