namespace bomber_man;
using Cairo;
using Gtk;
using Gdk;
using Color = Cairo.Color;
using Key = Gdk.Key;


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
        int scale = 2;
        
        for (int i = 0; i < 13; i++)
        {
            for (int j = 0; j < 11; j++)
            {
                Tiles tile = grid[i, j];
                ImageSurface surface = tileSet.Surfaces[tile];
                c.Save();
                c.Translate(j * TileSize * scale, i * TileSize * scale);
                c.Scale(scale, scale);
                c.SetSourceSurface(surface, 0, 0);
                c.Paint();
                
                c.Restore();
            }
        }
        
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
        
    }
    
}