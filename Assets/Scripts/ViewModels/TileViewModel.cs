using UnityEngine.Tilemaps;
using UnityEngine;
using Unity.VisualScripting;

public class TileViewModel : IViewModel
{
    public ObservableProperty<Color> DebugColor {get; set;} = new();
    public ObservableProperty<Sprite> Sprite {get; set;} = new();

    // TODO: Do I need to know the point here?
    public Point Point {get; private set;}
    public ShipRenderDef RenderDef {get; private set;}

    public TileViewModel(Point point)
    {
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
}