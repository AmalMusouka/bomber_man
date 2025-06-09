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
    public PlayerAnimator player2_animator;
    public BombAnimator bomb_animator;
    private Tiles[,] grid;
    private TileSet tileSet;
    private int rows = 13;
    private int cols = 11;
    private const int tile_width = GameConfig.TILE_WIDTH;
    private const int tile_height = GameConfig.TILE_HEIGHT;
    public List<(int, int)> tiles = new List<(int, int)>();

    public View(Game game)
    {
        this.game = game;
        player1_animator = new PlayerAnimator();
        player2_animator = new PlayerAnimator();
        bomb_animator = new BombAnimator();
        tileSet = new TileSet();
        SetSizeRequest(tile_width * rows, tile_height * cols);
        AddEvents((int)EventMask.AllEventsMask);
        GLib.Timeout.Add(16, () =>
        {
            player1_animator.UpdateAnimation();
            player2_animator.UpdateAnimation();
            bomb_animator.UpdateAnimation();
            QueueDraw();  
            return true;   
        });
    }

    public void DrawPlayer(Player player, PlayerAnimator player_animator, BombAnimator bomb_animator, Context c, bool player_up, bool player_down, bool player_left, bool player_right, string player_last_direction, bool color)
    {
        if (player.current_bomb != null)
        {
            var bomb = player.current_bomb;
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


        if (player != null)
        {
            if (player.is_dead)
            {
                player_animator.SetAnimation("death");

                if (player_animator.PlayerDeathEnd())
                {
                    player = null;
                    return;
                }
            }
            else
            {
                if (player_down)
                {
                    player_animator.SetAnimation("move_down");
                }
                else if (player_left)
                {
                    player_animator.SetAnimation("move_left");
                }
                else if (player_right)
                {
                    player_animator.SetAnimation("move_right");
                }
                else if (player_up)
                {
                    player_animator.SetAnimation("move_up");
                }
                else
                {
                    player_animator.SetAnimation("idle_" + player_last_direction);
                }
            }

            var sprite = player_animator.GetCurrentFrame();

            c.Save();
            c.Translate(player.player_x, player.player_y);
            c.Scale((double)tile_width / sprite.Width, (double)tile_height / sprite.Height);
            c.SetSourceSurface(sprite, 0, 0);
            c.Paint();
            if (color)
            {
                c.SetSourceRGBA(1.0, 0.0, 0.0, 0.6); // red, 50% transparent
                c.MaskSurface(sprite, 0, 0);
            }
            c.Restore();
        }
    }

    public void OnKeyPressEvent(object o, KeyPressEventArgs args)
    {
        Console.WriteLine("Key pressed" + args.Event.Key.ToString());
        switch (args.Event.Key)
        {
            // player 1
            case Key.a :
                game.player1_left = true;
                game.player1_last_direction = "left";
                break;
            case Key.d :
                game.player1_right = true;
                game.player1_last_direction = "right";
                break;
            case Key.w :
                game.player1_up = true;
                game.player1_last_direction = "up";
                break;
            case Key.s :
                game.player1_down = true;
                game.player1_last_direction = "down";
                break;
            case Key.Control_L :
                game.player1.PlaceBomb();
                break;
            
            // player 2
            case Key.j :
                game.player2_left = true;
                game.player2_last_direction = "left"; 
                break;
            case Key.l :
                game.player2_right = true;
                game.player2_last_direction = "right";
                break;
            case Key.i :
                game.player2_up = true;
                game.player2_last_direction = "up";
                break;
            case Key.k :
                game.player2_down = true;
                game.player2_last_direction = "down";
                break;
            case Key.u :
                game.player2.PlaceBomb();
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
                game.player1_up = false;
                break;
            case Key.s :
                game.player1_down = false;
                break;

            // player 2
            case Key.j :
                game.player2_left = false;
                break;
            case Key.l :
                game.player2_right = false;
                break;
            case Key.i :
                game.player2_up = false;
                break;
            case Key.k :
                game.player2_down = false;
                break;
        }   
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

        if (game.player1 != null)
        {
            DrawPlayer(game.player1, player1_animator, bomb_animator, c, game.player1_up, game.player1_down,
                game.player1_left, game.player1_right, game.player1_last_direction, false);
        }

        if (game.player2 != null)
        {
            DrawPlayer(game.player2, player2_animator, bomb_animator, c, game.player2_up, game.player2_down,
                game.player2_left, game.player2_right, game.player2_last_direction, true);
        }

        return true;
    }
}