using Gdk;
using Gtk;
using Cairo;
using Color = Cairo.Color;
using System;

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
class Game
{
    private int rows = 13;
    private int cols = 11;
    public int[,] grid;

    
    public Game()
    {
        grid = new int[rows, cols];
    }
}
class View : DrawingArea
{

    Color backgroundColor = new Color(173, 173, 173);
    //ImageSurface player = new ImageSurface("sprites/player.png");
    private Tiles[,] grid;
    private TileSet tileSet;
    private int rows = 13;
    private int cols = 11;
    private const int TileSize = 16;

    public View()
    {
        grid = new Tiles[rows, cols];
        tileSet = new TileSet();
        SetSizeRequest(TileSize * rows, TileSize * cols);

        InitializeGrid();
        AddEvents((int)EventMask.AllEventsMask);

    }

    void InitializeGrid()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if ( (i == 0 || i == rows - 1 || j == 0 || j == cols - 1) || ( i % 2 == 0 && j % 2 == 0 ))
                {
                    grid[i, j] = Tiles.UnbreakableWall;
                }
            }
        }
    }

    protected override bool OnDrawn(Context c)
    {

        double scale = 3.0;
        int gridWidth = rows * TileSize;
        int gridHeight = cols * TileSize;
        
        int areaWidth = Allocation.Width;
        int areaHeight = Allocation.Height;
        
        double offsetX = (areaWidth - gridWidth) / 2.0;
        double offsetY = (areaHeight - gridHeight) / 2.0;

        c.Save();
        c.Translate(offsetX, offsetY);
        
        
        for (int i = 0; i < 13; i++)
        {
            for (int j = 0; j < 11; j++)
            {
                Tiles tile = grid[i, j];
                ImageSurface surface = tileSet.Surfaces[tile];
                c.SetSourceSurface(surface, i * TileSize, j * TileSize);
                c.Paint();
            }
        }
        return true;
    }
    
}
class MyWindow : Gtk.Window
{
    
    public MyWindow() : base("BomberMan")
    {
        SetDefaultSize(13 * 16, 11 * 16);
        SetPosition(WindowPosition.CenterOnParent);
        Add(new View());
        ShowAll();
    }
    protected override bool OnDeleteEvent(Event e)
    {
        Application.Quit();
        return true;
    }
}

class Start
{
    static void Main()
    {
        Console.WriteLine("Working Directory: " + System.IO.Directory.GetCurrentDirectory());
        Application.Init();
        MyWindow win = new MyWindow();
        Application.Run();
    }
}