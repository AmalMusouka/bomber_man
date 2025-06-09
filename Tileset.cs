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
    private string sprite_dir;

    public TileSet()
    {
        sprite_dir = System.IO.Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName).FullName, "sprites");        

        Surfaces = new Dictionary<Tiles, ImageSurface>()
        {
            { Tiles.Floor, new ImageSurface(System.IO.Path.Combine(sprite_dir, "floor.png"))},
            { Tiles.BreakableWall, new ImageSurface(System.IO.Path.Combine(sprite_dir, "breakable_wall.png"))},
            { Tiles.UnbreakableWall, new ImageSurface(System.IO.Path.Combine(sprite_dir, "unbreakable_wall.png"))},
            { Tiles.Bomb, new ImageSurface(System.IO.Path.Combine(sprite_dir, "bomb_1.png"))}
        };
        
    }
}
