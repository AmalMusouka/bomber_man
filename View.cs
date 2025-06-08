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
    public PlayerAnimator player1_animator;
    public BombAnimator bomb_animator;
    private Tiles[,] grid;
    private TileSet tileSet;
    private int rows = 13;
    private int cols = 11;
    private const int tile_width = GameConfig.TILE_WIDTH;
    private const int tile_height = GameConfig.TILE_HEIGHT;
    public List<(int, int)> tiles = new List<(int, int)>();

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
            case Key.Control_L :
                game.player1.PlaceBomb();
                game.player1_bomb = true;
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
        bomb_animator = new BombAnimator();
        tileSet = new TileSet();
        SetSizeRequest(tile_width * rows, tile_height * cols);
        AddEvents((int)EventMask.AllEventsMask);

    }

    protected override bool OnDrawn(Context c)
    {
        for (int i = 0; i < 13; i++)
        {
            for (int j = 0; j < 11; j++)
            {
                Tiles tile = (Tiles)game.grid[i, j];
                ImageSurface surface = tileSet.Surfaces[tile];
                c.Save();
                c.Translate(j * tile_width, i * tile_height);
                c.Scale((double)tile_width / surface.Width, (double)tile_height / surface.Height);
                c.SetSourceSurface(surface, 0, 0);
                c.Paint();
                c.Restore();
            }
        }

        var bomb = game.player1.current_bomb;
        
        if (bomb != null)
        {
            //var bomb_sprite = new ImageSurface("/home/amalmusouka/bomber_man/sprites/bomb_1.png");
            bomb_animator.SetAnimation(bomb.exploded ? "explosion" : "bomb");
            var bomb_sprite = bomb_animator.GetCurrentFrame();
            
            c.Save();
            c.Translate(bomb.x, bomb.y);
            c.Scale((double)tile_width / bomb_sprite.Width, (double)tile_height / bomb_sprite.Height);
            c.SetSourceSurface(bomb_sprite, 0, 0);
            c.Paint();
            c.Restore();

            if (bomb.exploded && bomb_animator.ExplosionEnded())
            {
                bomb.explosion_finished = true;
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
        c.Translate(game.player1.player_x , game.player1.player_y );
        c.Scale((double)tile_width / sprite.Width, (double)tile_height / sprite.Height); 
        c.SetSourceSurface(sprite, 0, 0);
        c.Paint();
        c.Restore();
        return true;
    }
    
}