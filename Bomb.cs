namespace bomber_man;

public class Bomb
{
    public int x;
    public int y;
    public int timer = 3000;
    public DateTime placed_time;
    
    public bool exploded = false;
    public bool explosion_finished = false;

    public int tile_x;
    public int tile_y;
    
    public Bomb(int bomb_x, int bomb_y, int time = 3000)
    {
        x = bomb_x;
        y = bomb_y;
        timer = time;
        placed_time = DateTime.Now;

        tile_x = x / GameConfig.TILE_HEIGHT;
        tile_y = y / GameConfig.TILE_HEIGHT;
    }

    public bool Check()
    {
        Console.WriteLine(DateTime.Now - placed_time);
        if (!exploded && (DateTime.Now - placed_time).TotalMilliseconds >= timer)
        {
            Explode();
            return true;
        }
        return false;
    }

    public void Explode()
    {
        exploded = true;
    }

    public System.Drawing.Rectangle ExplosionRect()
    {
        return new System.Drawing.Rectangle(tile_x * GameConfig.TILE_WIDTH, tile_y * GameConfig.TILE_WIDTH, GameConfig.TILE_HEIGHT, GameConfig.TILE_WIDTH);
    }
}