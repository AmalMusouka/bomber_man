namespace bomber_man;
using Cairo;

public class Player
{
    
    public int player_x, player_y;
    private int speed = 5;     // horizontal velocity

    public Player(int start_x, int start_y)
    {
        player_x = start_x;
        player_y = start_y;
    }

    public Rectangle CollisionBox()
    {
        return new Rectangle(player_x, player_y, GameConfig.TILE_WIDTH,  GameConfig.TILE_HEIGHT);
    }

    public void Move(bool move_left, bool move_right, bool move_up, bool move_down, Func<System.Drawing.Rectangle, bool> collision_check, ImageSurface sprite, int scale)
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

        System.Drawing.Rectangle rect = new System.Drawing.Rectangle(new_player_x, new_player_y, GameConfig.TILE_WIDTH, GameConfig.TILE_HEIGHT);
        if (!collision_check(rect))
        {
            player_x = new_player_x;
            player_y = new_player_y;
        }
        else
        {
            Console.WriteLine("Collision Detected");
        }
    }
}