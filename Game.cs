using Cairo;

namespace bomber_man;
public class Game
{
    private int rows = 13;
    private int cols = 11;
    public int[,] grid;
    public Player player1;
    public Player player2;
    PlayerAnimator player1_animator;
    PlayerAnimator player2_animator;
    BombAnimator bomb_animator;

    public bool player1_left, player1_right, player1_up, player1_down;
    public bool player2_left, player2_right, player2_up, player2_down;
    public string player1_last_direction = "down";
    public string player2_last_direction = "up";

    public Game()
    {
        grid = new int[rows, cols];
        InitializeGrid();
        player1 = new Player(1 * GameConfig.TILE_WIDTH, 1 * GameConfig.TILE_HEIGHT, this);
        player1_animator = new PlayerAnimator();
        player2 = new Player(1 * GameConfig.TILE_WIDTH, 1 * GameConfig.TILE_HEIGHT, this);
        player2_animator = new PlayerAnimator();
        bomb_animator = new BombAnimator();
    }
    
    public void Tick(ImageSurface sprite)
    {

        if (player1 == null || player1.is_dead)
        {
            return;
        }

        if (player2 == null || player2.is_dead)
        {
            return;
        }

        player1.Move(player1_left, player1_right, player1_up, player1_down, (rect) => CollisionCheck(rect, player1), sprite);
        player2.Move(player2_left, player2_right, player2_up, player2_down, (rect) => CollisionCheck(rect, player2), sprite);
        
        if (player1.current_bomb != null)
        {
            player1.current_bomb.Check();
            
            if (player1.current_bomb.explosion_finished)
            {
                player1.current_bomb = null;
            }
        }

        if (player2.current_bomb != null)
        {
            player2.current_bomb.Check();

            if (player2.current_bomb.explosion_finished)
            {
                player2.current_bomb = null;
            }
        }
    }
    
    void InitializeGrid()
    {
        Random rand = new Random();
        int max_breakable_walls = 10;
        int breakable_walls = 0;
        List<(int i, int j)> open_positions = new List<(int, int)>();
        
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if ( (i == 0 || i == rows - 1 || j == 0 || j == cols - 1) || ( i % 2 == 0 && j % 2 == 0 ))
                {
                    grid[i, j] = (int)Tiles.UnbreakableWall;
                }
                else
                {
                    bool spawn_check = ((i <= 1 && j <= 1) || (i <= 1 && j >= cols - 2 ) || (i >= rows - 2 && j <= 1) ) || (i >= rows - 2 && j >= cols - 2);

                    if (!spawn_check)
                    {
                        open_positions.Add((i, j));
                    }
                }
            }
        }
        
        open_positions = open_positions.OrderBy(_ => rand.Next()).ToList();

        for (int k = 0; k < Math.Min(max_breakable_walls, open_positions.Count); k++)
        {
            var (i, j) = open_positions[k];
            grid[i, j] = (int)Tiles.BreakableWall;
        }
    }
    public bool CollisionCheck(System.Drawing.Rectangle player_rect, Player player)
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (grid[i, j] == (int)Tiles.UnbreakableWall || grid[i, j] == (int)Tiles.BreakableWall)
                {
                    var rect = new System.Drawing.Rectangle(j * GameConfig.TILE_WIDTH, i * GameConfig.TILE_HEIGHT, GameConfig.TILE_WIDTH, GameConfig.TILE_HEIGHT);
                    if (rect.IntersectsWith(player_rect))
                    {
                        return true;
                    }
                }
            }
        }
        
        if (player1.current_bomb != null && player1.current_bomb.exploded && !player1.current_bomb.explosion_finished)
        {
            if (player1.current_bomb.ExplosionRect().IntersectsWith(player_rect))
            {
                if (!player.is_dead)
                {
                    player.is_dead = true;
                }
                return true;
            }
        }
        
        if (player2.current_bomb != null && player2.current_bomb.exploded && !player2.current_bomb.explosion_finished)
        {
            if (player2.current_bomb.ExplosionRect().IntersectsWith(player_rect))
            {
                if (!player.is_dead)
                {
                    player.is_dead = true;
                }
                return true;
            }
        }
        return false;
    }

}



