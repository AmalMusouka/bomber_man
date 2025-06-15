namespace bomber_man;
using Cairo;
using Gtk;
using Gdk;

class View : DrawingArea
{
    Game game;
    public PlayerAnimator player1_animator;
    public PlayerAnimator player2_animator;
    public BombAnimator bomb_animator_player_1;
    public BombAnimator bomb_animator_player_2;
    private Tiles[,] grid;
    private TileSet tileSet;
    private int rows = 13;
    private int cols = 11;
    private const int tile_width = GameConfig.TILE_WIDTH;
    private const int tile_height = GameConfig.TILE_HEIGHT;

    /// <summary>
    /// View constructor consists of all the animation objects and states as well as tiles
    /// </summary>
    public View(Game game)
    {
        this.game = game;
        player1_animator = new PlayerAnimator();
        player2_animator = new PlayerAnimator();
        bomb_animator_player_1 = new BombAnimator();
        bomb_animator_player_2 = new BombAnimator();
        tileSet = new TileSet();
        SetSizeRequest(tile_width * cols, tile_height * rows);
        AddEvents((int)EventMask.AllEventsMask);
        GLib.Timeout.Add(16, () =>
        {
            player1_animator.UpdateAnimation();
            player2_animator.UpdateAnimation();
            bomb_animator_player_1.UpdateAnimation();
            bomb_animator_player_2.UpdateAnimation();
            QueueDraw();  
            return true;   
        });
    }
    

    /// <summary>
    /// Draws the player and all player related objects and animations
    /// </summary>
    public void DrawPlayer(Player player, PlayerAnimator player_animator, BombAnimator bomb_animator, Context c, bool player_up, bool player_down, bool player_left, bool player_right, string player_last_direction, bool color)
    {
        
        if (player.current_bomb != null)
        {
            var bomb = player.current_bomb;
            if (!bomb.exploded)
            {
                bomb_animator.SetAnimation("bomb");
                var bomb_sprite = bomb_animator.GetCurrentFrame();

                c.Save();
                c.Translate(bomb.x, bomb.y);
                c.Scale((double)tile_width / bomb_sprite.Width, (double)tile_height / bomb_sprite.Height);
                c.SetSourceSurface(bomb_sprite, 0, 0);
                c.Paint();
                c.Restore();
            }
            else
            {
                bomb_animator.SetAnimation("explosion");
                var explosion_center = bomb_animator.GetExplosionFrame("horizontal_explosion_center");

                // Draw center explosion
                c.Save();
                c.Translate(bomb.tile_x * GameConfig.TILE_WIDTH, bomb.tile_y * GameConfig.TILE_HEIGHT);
                c.Scale((double)GameConfig.TILE_WIDTH / explosion_center.Width,
                    (double)GameConfig.TILE_HEIGHT / explosion_center.Height);
                c.SetSourceSurface(explosion_center, 0, 0);
                c.Paint();
                c.Restore();
                
                foreach (var (dx, dy) in bomb.explosion_tiles)
                {

                    string key;

                    if (dx == bomb.tile_x && dy == bomb.tile_y)
                    {
                        key = "horizontal_explosion_center";
                    }
                    else if (dx < bomb.tile_x)
                    {
                        key = (dx == bomb.tile_x - 1 || !bomb.explosion_tiles.Contains((dx - 1, dy)))
                            ? "horizontal_explosion_left"
                            : "horizontal_explosion";
                    }
                    else if (dx > bomb.tile_x)
                    {
                        key = (dx == bomb.tile_x + 1 || !bomb.explosion_tiles.Contains((dx + 1, dy)))
                            ? "horizontal_explosion_right"
                            : "horizontal_explosion";
                    }
                    else if (dy < bomb.tile_y)
                    {
                        key = (dy == bomb.tile_y - 1 || !bomb.explosion_tiles.Contains((dx, dy - 1)))
                            ? "vertical_explosion_up"
                            : "vertical_explosion";
                    }
                    else 
                    {
                        key = (dy == bomb.tile_y + 1 || !bomb.explosion_tiles.Contains((dx, dy + 1)))
                            ? "vertical_explosion_down"
                            : "vertical_explosion";
                    }

                    var frame = bomb_animator.GetExplosionFrame(key);

                    c.Save();
                    c.Translate(dx * GameConfig.TILE_WIDTH, dy * GameConfig.TILE_HEIGHT);
                    c.Scale((double)GameConfig.TILE_WIDTH / frame.Width,
                        (double)GameConfig.TILE_HEIGHT / frame.Height);
                    c.SetSourceSurface(frame, 0, 0);
                    c.Paint();
                    c.Restore();
                }

                if (bomb.exploded && bomb_animator.ExplosionEnded())
                {
                    bomb.explosion_finished = true;
                }
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
            if (color) // player 2
            {
                c.SetSourceRGBA(1.0, 0.0, 0.0, 0.6); // red
                c.MaskSurface(sprite, 0, 0);
            }
            c.Restore();
        }
    }

    public void OnKeyPressEvent(object o, KeyPressEventArgs args)
    {
        game.OnKeyPress(args.Event.Key);
    }

    public void OnKeyReleaseEvent(object o, KeyReleaseEventArgs args)
    {
        game.OnKeyRelease(args.Event.Key);
    }

    /// <summary>
    /// Draws the tiles and every player as well as player objects
    /// </summary>
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
            var player_1_states = game.player_movement_states[0];
            DrawPlayer(
                game.player1,
                player1_animator,
                bomb_animator_player_1,
                c,
                player_up: player_1_states[(int)Game.binds.up],
                player_down: player_1_states[(int)Game.binds.down],
                player_left: player_1_states[(int)Game.binds.left],
                player_right: player_1_states[(int)Game.binds.right],
                player_last_direction: game.player1_last_direction,
                color: false);
        }

        if (game.player2 != null)
        {
            var player_2_states = game.player_movement_states[1];
            DrawPlayer(
                game.player2,
                player2_animator,
                bomb_animator_player_2,
                c,
                player_up: player_2_states[(int)Game.binds.up],
                player_down: player_2_states[(int)Game.binds.down],
                player_left: player_2_states[(int)Game.binds.left],
                player_right: player_2_states[(int)Game.binds.right],
                player_last_direction: game.player2_last_direction,
                color: true);
        }
        
        if (game.winner != null)
        {
            c.SetSourceRGBA(0, 0, 0, 0.5);
            c.Rectangle(0, 0, Allocation.Width, Allocation.Height);
            c.Fill();
            c.SetSourceRGB(1, 1, 1);
            c.SelectFontFace("Sans", FontSlant.Normal, FontWeight.Bold);
            c.SetFontSize(36);

            string message = $"{game.winner} Wins!";
            TextExtents extents = c.TextExtents(message);

            double x = (Allocation.Width - extents.Width) / 2 - extents.XBearing;
            double y = (Allocation.Height - extents.Height) / 2 - extents.YBearing;
            c.MoveTo(x, y);
            c.ShowText(message);
            c.Stroke();
        }

        return true;
    }
}