namespace bomber_man;
using Cairo;

public class Player
{
    
    public int player_x, player_y;
    public Bomb current_bomb = null;
    public bool has_bomb = true;
    private int speed = 5;     // horizontal velocity

    public Player(int start_x, int start_y)
    {
        player_x = start_x;
        player_y = start_y;
    }

    public void Move(bool move_left, bool move_right, bool move_up, bool move_down, Func<System.Drawing.Rectangle, bool> collision_check, ImageSurface sprite)
    {

        int new_player_x = player_x;
        int new_player_y = player_y;
        
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

    public void PlaceBomb()
    {
        Console.WriteLine("Space pressed");
        if (current_bomb == null)
        {
            int bomb_x = (player_x / GameConfig.TILE_WIDTH) * GameConfig.TILE_WIDTH;
            int bomb_y = (player_y / GameConfig.TILE_HEIGHT) * GameConfig.TILE_HEIGHT;

            current_bomb = new Bomb(bomb_x, bomb_y);
            Console.WriteLine("Bomb placed at " + bomb_x + ", " + bomb_y);
        }
    }
}