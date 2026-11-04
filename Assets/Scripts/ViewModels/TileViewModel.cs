using UnityEngine.Tilemaps;
using UnityEngine;
using Unity.VisualScripting;

public class TileViewModel : IViewModel
{
    public ObservableProperty<Color> DebugColor {get; set;} = new();
    public ObservableProperty<Sprite> Sprite {get; set;} = new();

    public Point Point {get; private set;}
    public ShipRenderDef RenderDef {get; private set;}

    private IGameManager _gameManager;

    public TileViewModel(Point point, IGameManager gameManager)
    {
        _gameManager = gameManager;

        Point = point;
    }

    public void SetRenderDef(ShipRenderDef renderDef)
    {
        if (renderDef == null)
            return;
        
        RenderDef = renderDef;

        DebugColor.SetAsMutable(RenderDef.DebugColor);
        Sprite.SetAsMutable(RenderDef.ShipSprite);
    }

    public void Reset()
    {   
        DebugColor.SetAsMutable(Color.white);
        Sprite.SetAsMutable(null);
    }

    public bool CheckForShipHit()
    {
        if (_gameManager == null)
        {
            this.LogError("Missing _gameManager instance.");
            return false;
        }

        return _gameManager.CheckForShipHit(Point);;
    }
}