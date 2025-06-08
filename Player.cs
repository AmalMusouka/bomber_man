namespace bomber_man;
using Cairo;

public class Player
{
    
    public int player_x, player_y;
    private int speed = 2;     // horizontal velocity

    public Player(int start_x, int start_y)
    {
        player_x = start_x;
        player_y = start_y;
    }

    public Rectangle CollisionBox(ImageSurface sprite, int scale)
    {
        return new Rectangle(player_x, player_y, sprite.Width * scale, sprite.Height * scale);
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