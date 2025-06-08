namespace bomber_man;
using Cairo;

enum Tiles
{
    Floor,
    BreakableWall,
    UnbreakableWall,
    Bomb
}

class TileSet
{
    public Dictionary<Tiles, ImageSurface> Surfaces;

    public TileSet()
    {
        Surfaces = new Dictionary<Tiles, ImageSurface>()
        {
            { Tiles.Floor, new ImageSurface("/home/amalmusouka/bomber_man/sprites/floor.png")},
            { Tiles.BreakableWall, new ImageSurface("/home/amalmusouka/bomber_man/sprites/breakableWall.png")},
            { Tiles.UnbreakableWall, new ImageSurface("/home/amalmusouka/bomber_man/sprites/unbreakableWall.png")},
            { Tiles.Bomb, new ImageSurface("/home/amalmusouka/bomber_man/sprites/bomb.png")}
        };
        
    }
}
