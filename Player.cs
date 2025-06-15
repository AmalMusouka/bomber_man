namespace bomber_man;
using Cairo;

public class Player
{
    private int speed = 5;     // horizontal velocity

    public int player_x, player_y;
    public Bomb current_bomb = null;
    public bool is_dead = false;
    Game game;

    public Player(int start_x, int start_y, Game game)
    {
        player_x = start_x;
        player_y = start_y;
        this.game = game;
    }
    
    /// <summary>
    /// Handles player movement
    /// </summary>
    public void Move(bool move_left, bool move_right, bool move_up, bool move_down, Func<System.Drawing.Rectangle, bool> collision_check, ImageSurface sprite)
    {

        int new_player_x = player_x;
        int new_player_y = player_y;

        if (is_dead)
        {
            return;
        }
        if (move_left)
        {
            new_player_x -= speed;
        }
        else if (move_right)
        {
            new_player_x += speed;
        }
        else if (move_up)
        {
            new_player_y -= speed;
        }
        else if (move_down)
        {
            new_player_y += speed;
        }

        int box_width = GameConfig.TILE_WIDTH - 32;
        int box_height = GameConfig.TILE_HEIGHT / 3;
        int offset_x = (GameConfig.TILE_WIDTH - box_width) / 2;
        int offset_y = GameConfig.TILE_HEIGHT - box_height;
        
        System.Drawing.Rectangle rect = new System.Drawing.Rectangle(new_player_x + offset_x, new_player_y + offset_y, box_width, box_height);
        if (!collision_check(rect))
        {
            player_x = new_player_x;
            player_y = new_player_y;
        }
    }

    /// <summary>
    /// handles player bomb placement
    /// </summary>
    public void PlaceBomb()
    {
        
        int player_center_x = player_x + GameConfig.TILE_WIDTH / 2;
        int player_center_y = player_y + GameConfig.TILE_HEIGHT / 2;
        
        int tile_x = player_center_x / GameConfig.TILE_WIDTH;
        int tile_y = player_center_y / GameConfig.TILE_HEIGHT;

        if (current_bomb == null)
        {
            if (tile_x >= 0 && tile_x < game.grid.GetLength(1) && tile_y >= 0 && tile_y < game.grid.GetLength(0))
            {
                int tile = game.grid[tile_x, tile_y];
                if (tile != (int)Tiles.UnbreakableWall && tile != (int)Tiles.BreakableWall)
                {

                    int bomb_x = tile_x * GameConfig.TILE_WIDTH;
                    int bomb_y = tile_y * GameConfig.TILE_HEIGHT;

                    current_bomb = new Bomb(bomb_x, bomb_y);

                }
            }
        }
    }
}