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
    private int speed = 2;     // horizontal velocity

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
    public string last_direction = "down";

    public void Tick()
    {
        player1.Move(player1_left, player1_right, player2_up, player1_down);
    }

    
    public Game()
    {
        grid = new int[rows, cols];
    }
}

class PlayerAnimator
{
    private Dictionary<string, ImageSurface[]> animations;
    private int currentFrame = 0;
    private int frameCounter = 0;
    private int frameSpeed = 6; // ticks per switch

    private string currentAnimation = "idle_down";

    public PlayerAnimator()
    {
        animations = new Dictionary<string, ImageSurface[]>
        {
            {
                "idle_down", [
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/bomberman_idle_down.png")
                ]
            },
            {
                "idle_left", [
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/bomberman_idle_left.png")
                ]
            },
            {
                "idle_right", [
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/bomberman_idle_right.png")
                ]
            },
            {
                "idle_up", [
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/bomberman_idle_up.png")
                ]
            },
            {
                "move_down", [
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/bomberman_move_down1.png"),
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/bomberman_idle_down.png"),
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/bomberman_move_down2.png"),
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/bomberman_idle_down.png"),
                ]
            },
            {
                "move_right", [
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/bomberman_move_right1.png"),
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/bomberman_idle_right.png"),
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/bomberman_move_right2.png"),
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/bomberman_idle_right.png"),
                ]
            },
            {
                "move_left", [
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/bomberman_move_left1.png"),
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/bomberman_idle_left.png"),
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/bomberman_move_left2.png"),
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/bomberman_idle_left.png"),
                ]   
            },
            {
                "move_up", [
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/bomberman_move_up1.png"),
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/bomberman_idle_up.png"),
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/bomberman_move_up2.png"),
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/bomberman_idle_up.png"),
                ]   
            }
            
        };

    }

    public void SetAnimation(string animation)
    {
        if (currentAnimation != animation)
        {
            currentAnimation = animation;
            currentFrame = 0;
            frameCounter = 0;
        }
    }

    public ImageSurface GetCurrentFrame()
    {
        var frames = animations[currentAnimation];
        if (frames.Length == 1)
        {
            return frames[0];
        }
        
        frameCounter++;

        if (frameCounter >= frameSpeed)
        {
            frameCounter = 0;
            currentFrame = (currentFrame + 1) % frames.Length;
        }
        return frames[currentFrame];
    }
}
class View : DrawingArea
{
    Game game;
    Color backgroundColor = new Color(173, 173, 173);
    ImageSurface player1 = new ImageSurface("/home/amalmusouka/bomber_man/sprites/bomberman_idle_down.png");
    private PlayerAnimator player1_animator;
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
                game.last_direction = "left";
                break;
            case Key.d :
                game.player1_right = true;
                game.last_direction = "right";
                break;
            case Key.w :
                game.player2_up = true;
                game.last_direction = "up";
                break;
            case Key.s :
                game.player1_down = true;
                game.last_direction = "down";
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
        player1_animator = new PlayerAnimator();
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
        if (game.player1_down)
        {
            player1_animator.SetAnimation("move_down");
        }
        else if (game.player1_left)
        {
            player1_animator.SetAnimation("move_left");
        }
        else if (game.player1_right)
        {
            player1_animator.SetAnimation("move_right");
        }
        else if (game.player2_up)
        {
            player1_animator.SetAnimation("move_up");
        }
        else
        {
            player1_animator.SetAnimation("idle_" + game.last_direction);
        }
        
        var sprite = player1_animator.GetCurrentFrame();
        c.Save();
        c.Translate(game.player1.player_x, game.player1.player_y);
        c.Scale(2.0, 2.0); 
        c.SetSourceSurface(sprite, game.player1.player_x, game.player1.player_y);
        c.Paint();
        c.Restore();
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
        Application.Init();
        MyWindow win = new MyWindow();
        Application.Run();
    }
}