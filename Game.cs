namespace bomber_man;
class Game
{
    private int rows = 13;
    private int cols = 11;
    public int[,] grid;
    public Player player1 = new Player(100, 100);

    public bool player1_left, player1_right, player2_up, player1_down;
    public string last_direction = "down";

    public void Tick()
    {
        player1.Move(player1_left, player1_right, player2_up, player1_down);
    }


    public Game()
    {
        grid = new int[rows, cols];
    }

    public bool CollisionCheck(System.Drawing.Rectangle player_rect, int scale, int TileSize)
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (grid[i, j] == (int)Tiles.UnbreakableWall || grid[i, j] == (int)Tiles.BreakableWall)
                {
                    var rect = new System.Drawing.Rectangle(j * TileSize * scale, i * TileSize * scale, TileSize * scale, TileSize * scale);
                    if (rect.IntersectsWith(player_rect))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

}



