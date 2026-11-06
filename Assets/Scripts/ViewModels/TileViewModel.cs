using UnityEngine.Tilemaps;
using UnityEngine;
using Unity.VisualScripting;

public class TileViewModel : IViewModel
{
    #region Constants
    private const float StartingCoverAlpha = 0.5f;
    #endregion

    #region Observable Properties
    public ObservableProperty<Color> DebugColor {get; set;} = new();
    public ObservableProperty<Sprite> Sprite {get; set;} = new();
    public ObservableProperty<float> CoverCanvasGroupAlpha {get; set;} = new();
    public ObservableProperty<bool> IsButtonInteractable {get; set;} = new();
    #endregion

    #region Properties
    public Point Point {get; private set;}
    public ShipRenderDef RenderDef {get; private set;}
    #endregion

    #region Private Variables
    private IGameManager _gameManager;
    #endregion

    #region Public Functions
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

    public void SetCoverImageAlpha(float alpha)
    {
        CoverCanvasGroupAlpha.SetAsMutable(alpha);
    }

    public void SetIsButtonInteractable(bool isInteractable)
    {
        IsButtonInteractable.SetAsMutable(isInteractable);
    }

    public void Reset()
    {   
        DebugColor.SetAsMutable(Color.white);
        Sprite.SetAsMutable(null);
        SetIsButtonInteractable(true);
        SetCoverImageAlpha(StartingCoverAlpha);
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
    #endregion
}