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
    public List<(int x, int y)> explosion_tiles = new();
    
    public Bomb(int bomb_x, int bomb_y, int time = 3000)
    {
        x = bomb_x;
        y = bomb_y;
        timer = time;
        placed_time = DateTime.Now;

        tile_x = x / GameConfig.TILE_WIDTH;
        tile_y = y / GameConfig.TILE_HEIGHT;
    }

    /// <summary>
    /// Used to check whether the bomb has exploded if not, then wait until a certain time is up to initiate the explosion
    /// </summary>
    public void Check(int[,] grid)
    {
        if (!exploded && (DateTime.Now - placed_time).TotalMilliseconds >= timer)
        {
            Explode(grid);
        }
    }

    /// <summary>
    /// Explode the bomb and considers all the tiles nearby as reference for future use
    /// </summary>
    public void Explode(int[,] grid)
    {
        exploded = true;
        explosion_tiles.Clear();
        var (left, right, up, down) = CalculateExplosionRanges(grid, GameConfig.EXPLOSION_RANGE);
        explosion_tiles.Add((tile_x, tile_y)); // center

        for (int i = 1; i <= left; i++)
        {
            explosion_tiles.Add((tile_x - i, tile_y));
        }

        for (int i = 1; i <= right; i++)
        {
            explosion_tiles.Add((tile_x + i, tile_y));
        }

        for (int i = 1; i <= up; i++)
        {
            explosion_tiles.Add((tile_x, tile_y - i));
        }

        for (int i = 1; i <= down; i++)
        {
            explosion_tiles.Add((tile_x, tile_y + i));
        }

        foreach (var (x, y) in explosion_tiles)
        {
            if (x >= 0 && y >= 0 && y < grid.GetLength(0) && x < grid.GetLength(1))
            {
                if (grid[y, x] == (int)Tiles.BreakableWall)
                {
                    grid[y, x] = (int)Tiles.Floor;
                }
            }
        }
    }
    
    /// <summary>
    /// Check for nearby tiles so that the explosion does not go through them
    /// </summary>
    public (int left, int right, int up, int down) CalculateExplosionRanges(int[,] grid, int max_range)
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);

        int left = 0, right = 0, up = 0, down = 0;
        for (int i = tile_x - 1; i >= tile_x - max_range && i >= 0; i--)
        {
            if (grid[tile_y, i] == (int)Tiles.UnbreakableWall) break;
            left++;
            if (grid[tile_y, i] == (int)Tiles.BreakableWall) break;
        }

        for (int i = tile_x + 1; i <= tile_x + max_range && i < cols; i++)
        {
            if (grid[tile_y, i] == (int)Tiles.UnbreakableWall) break;
            right++;
            if (grid[tile_y, i] == (int)Tiles.BreakableWall) break;
        }

        for (int j = tile_y - 1; j >= tile_y - max_range && j >= 0; j--)
        {
            if (grid[j, tile_x] == (int)Tiles.UnbreakableWall) break;
            up++;
            if (grid[j, tile_x] == (int)Tiles.BreakableWall) break;
        }

        for (int j = tile_y + 1; j <= tile_y + max_range && j < rows; j++)
        {
            if (grid[j, tile_x] == (int)Tiles.UnbreakableWall) break;
            down++;
            if (grid[j, tile_x] == (int)Tiles.BreakableWall) break;
        }

        return (left, right, up, down);
    }
    /// <summary>
    /// Collision rect for the explosion
    /// </summary>
    public System.Drawing.Rectangle ExplosionRect()
    {
        return new System.Drawing.Rectangle(tile_x * GameConfig.TILE_WIDTH, tile_y * GameConfig.TILE_WIDTH, GameConfig.TILE_HEIGHT, GameConfig.TILE_WIDTH);
    }
}