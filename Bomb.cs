namespace bomber_man;

public class Bomb
{
    public int x;
    public int y;
    public int radius = 2;
    public int timer = 3000;
    public DateTime placed_time;
    
    public bool exploded = false;
    public bool explosion_finished = false;
    
    public Bomb(int bomb_x, int bomb_y, int explosion_radius = 2, int time = 3000)
    {
        x = bomb_x;
        y = bomb_y;
        radius = explosion_radius;
        timer = time;
        placed_time = DateTime.Now;
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
}