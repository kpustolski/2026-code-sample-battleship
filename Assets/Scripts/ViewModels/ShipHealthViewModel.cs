public class ShipHealthViewModel : IViewModel
{
    public ObservableProperty<int> Health = new();

    public Id<Ship> ShipId => _shipId;
    public int StartingHealth => _startingHealth;

    private Id<Ship> _shipId;
    private int _startingHealth;
    private IGameManager _gameManager;

    public ShipHealthViewModel(Id<Ship> shipId, int startingHealth, IGameManager gameManager)
    {
        _shipId = shipId;
        _gameManager = gameManager;
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
}