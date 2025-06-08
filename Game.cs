using Cairo;

namespace bomber_man;
class Game
{
    private int rows = 13;
    private int cols = 11;
    public int[,] grid;
    public Player player1 = new Player(100, 100);

    public bool player1_left, player1_right, player2_up, player1_down;
    public string last_direction = "down";

    public void Tick(ImageSurface sprite, int scale, int tile_size)
    {
        player1.Move(player1_left, player1_right, player2_up, player1_down, (rect) => CollisionCheck(rect), sprite, scale);
    }
    
    public Game()
    {
        grid = new int[rows, cols];
        InitializeGrid();
        player1 = new Player(1 * GameConfig.TILE_WIDTH, 1 * GameConfig.TILE_HEIGHT);
    }

    void InitializeGrid()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if ( (i == 0 || i == rows - 1 || j == 0 || j == cols - 1) || ( i % 2 == 0 && j % 2 == 0 ))
                {
                    grid[i, j] = (int)Tiles.UnbreakableWall;
                }
            }
        }
    }
    public bool CollisionCheck(System.Drawing.Rectangle player_rect)
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (grid[i, j] == (int)Tiles.UnbreakableWall || grid[i, j] == (int)Tiles.BreakableWall)
                {
                    var rect = new System.Drawing.Rectangle(j * GameConfig.TILE_WIDTH, i * GameConfig.TILE_HEIGHT, GameConfig.TILE_WIDTH - 24, GameConfig.TILE_HEIGHT - 24);
                    if (rect.IntersectsWith(player_rect))
                    {
                        Console.WriteLine("Collision Detected");
                        return true;
                    }
                }
            }
        }
        return false;
    }

}



