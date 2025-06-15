namespace bomber_man;
using Cairo;

enum Tiles
{
    Floor = 0,
    UnbreakableWall = 1,
    BreakableWall = 2,
    Bomb = 3
}

class TileSet
{
    public Dictionary<Tiles, ImageSurface> Surfaces;

    public TileSet()
    {
        Surfaces = new Dictionary<Tiles, ImageSurface>()
        {
            { Tiles.Floor, new ImageSurface("sprites/floor.png")},
            { Tiles.BreakableWall, new ImageSurface("sprites/breakable_wall.png")},
            { Tiles.UnbreakableWall, new ImageSurface("sprites/unbreakable_wall.png")},
            { Tiles.Bomb, new ImageSurface("sprites/bomb_1.png")}
        };
        
    }
}
