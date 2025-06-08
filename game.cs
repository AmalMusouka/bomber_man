using Gdk;
using Gtk;
using Cairo;
using Color = Cairo.Color;
using System;
using Key = Gdk.Key;
using Timeout = GLib.Timeout;

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

class Player
{
    public int player_x, player_y;
    private int speed = 5;     // horizontal velocity

    public Player(int start_x, int start_y)
    {
        player_x = start_x;
        player_y = start_y;
    }

    public void Move(bool move_left, bool move_right, bool move_up, bool move_down) {
        if (move_left)
        {
            player_x -= speed;
        }
        else if (move_right)
        {
            player_x += speed;
        }
        else if (move_up)
        {
            player_y -= speed;
        }
        else if (move_down)
        {
            player_y += speed;
        }
    
    }
}
class Game
{
    private int rows = 13;
    private int cols = 11;
    public int[,] grid;
    public Player player1 = new Player(100, 100);
    
    public bool player1_left, player1_right, player2_up, player1_down;

    public void Tick()
    {
        player1.Move(player1_left, player1_right, player2_up, player1_down);
    }

    
    public Game()
    {
        grid = new int[rows, cols];
    }
}
class View : DrawingArea
{
    Game game;
    Color backgroundColor = new Color(173, 173, 173);
    ImageSurface player1 = new ImageSurface("/home/amalmusouka/bomber_man/sprites/player.png");
    private Tiles[,] grid;
    private TileSet tileSet;
    private int rows = 13;
    private int cols = 11;
    private const int TileSize = 16;

    public void OnKeyPressEvent(object o, KeyPressEventArgs args)
    {
        switch (args.Event.Key)
        {
            // player 1
            case Key.a :
                game.player1_left = true;
                break;
            case Key.d :
                game.player1_right = true;
                break;
            case Key.w :
                game.player2_up = true;
                break;
            case Key.s :
                game.player1_down = true;
                break;
        }   
    }

    public void OnKeyReleaseEvent(object o, KeyReleaseEventArgs args)
    {
        switch (args.Event.Key)
        {
            // player 1
            case Key.a :
                game.player1_left = false;
                break;
            case Key.d :
                game.player1_right = false;
                break;
            case Key.w :
                game.player2_up = false;
                break;
            case Key.s :
                game.player1_down = false;
                break;
        }   
    }

    public View(Game game)
    {
        this.game = game;
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
        c.SetSourceSurface(player1, game.player1.player_x, game.player1.player_y );
        c.Paint();
        return true;
        /*double scale = 3.0;
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
        return true;*/
    }
    
}
class MyWindow : Gtk.Window
{
    Game game = new Game();
    View view;

    bool on_timeout()
    {
        game.Tick();
        QueueDraw();
        return true;
    }
    public MyWindow() : base("BomberMan")
    {
        SetDefaultSize(13 * 16, 11 * 16);
        SetPosition(WindowPosition.CenterOnParent);
        view = new View(game);
        Add(view);
        
        AddEvents((int)EventMask.KeyPressMask | (int)EventMask.KeyReleaseMask);

        KeyPressEvent += view.OnKeyPressEvent;
        KeyReleaseEvent += view.OnKeyReleaseEvent;
        
        Timeout.Add(30, on_timeout);
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