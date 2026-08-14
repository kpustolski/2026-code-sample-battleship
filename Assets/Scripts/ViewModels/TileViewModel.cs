public class TileViewModel : IViewModel
{
    public ObservableProperty<Point> Point {get; set;}
    public ObservableProperty<float> WorldPositionX {get; set;}
    public ObservableProperty<float> WorldPositionY {get; set;}

    public TileViewModel(Point point, float worldPosX, float worldPosY)
    {
        Point.SetAsMutable(point);
        WorldPositionX.SetAsMutable(worldPosX);
        WorldPositionY.SetAsMutable(worldPosY);
    }
}