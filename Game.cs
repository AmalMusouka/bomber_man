using Cairo;

namespace bomber_man;
using Key = Gdk.Key;

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

    public bool[][] player_movement_states;

    public enum binds
    {
        left = 0,
        right = 1,
        up = 2,
        down = 3,
        bomb = 4
    };

    public string player1_last_direction = "down";
    public string player2_last_direction = "down";
    public string winner = null;
    public List<(int x, int y)> destroy_tiles = new List<(int x, int y)>();


    // Player keys mapping
    public readonly (int player, Key key, binds dir)[] player_keys = new[]
    {
        (1, Key.a, binds.left),
        (1, Key.d, binds.right),
        (1, Key.w, binds.up),
        (1, Key.s, binds.down),
        (1, Key.Control_L, binds.bomb),

        (2, Key.j, binds.left),
        (2, Key.l, binds.right),
        (2, Key.i, binds.up),
        (2, Key.k, binds.down),
        (2, Key.u, binds.bomb)
    };

    /// <summary>
    /// Game constructor that holds all the necessary information
    /// </summary>
    public Game()
    {
        grid = new int[rows, cols];
        InitializeGrid();
        player1 = new Player(1 * GameConfig.TILE_WIDTH, 1 * GameConfig.TILE_HEIGHT, this);
        player2 = new Player((cols - 2) * GameConfig.TILE_WIDTH, 1 * GameConfig.TILE_HEIGHT, this);
        player_movement_states = new bool[2][];
        player_movement_states[0] = new bool[4];
        player_movement_states[1] = new bool[4];
        player1_animator = new PlayerAnimator();
        player2_animator = new PlayerAnimator();
        bomb_animator = new BombAnimator();
    }

    /// <summary>
    /// One update / step in game logic
    /// </summary>
    public void Tick(ImageSurface sprite)
    {

        destroy_tiles.Clear();

        if (player1 == null || player1.is_dead)
        {
            winner = "Player 2";
            return;
        }

        if (player2 == null || player2.is_dead)
        {
            winner = "Player 1";
            return;
        }

        player1.Move(
            player_movement_states[0][(int)Game.binds.left],
            player_movement_states[0][(int)Game.binds.right],
            player_movement_states[0][(int)Game.binds.up],
            player_movement_states[0][(int)Game.binds.down],
            rect => CollisionCheck(rect, player1),
            sprite);

        player2.Move(
            player_movement_states[1][(int)Game.binds.left],
            player_movement_states[1][(int)Game.binds.right],
            player_movement_states[1][(int)Game.binds.up],
            player_movement_states[1][(int)Game.binds.down],
            rect => CollisionCheck(rect, player2),
            sprite);

        if (player1.current_bomb != null)
        {
            player1.current_bomb.Check(grid);

            if (player1.current_bomb.explosion_finished)
            {
                player1.current_bomb = null;
            }
        }

        if (player2.current_bomb != null)
        {
            player2.current_bomb.Check(grid);

            if (player2.current_bomb.explosion_finished)
            {
                player2.current_bomb = null;
            }
        }
    }


    /// <summary>
    /// Key press handler for movement and bomb
    /// </summary>
    public void OnKeyPress(Key key)
    {
        foreach (var (player, mapped_key, bind) in player_keys)
        {
            if (key == mapped_key)
            {
                var movement_states = player_movement_states[player - 1];
                switch (bind)
                {
                    case binds.left:
                        movement_states[(int)binds.left] = true;
                        break;
                    case binds.right:
                        movement_states[(int)binds.right] = true;
                        break;
                    case binds.up:
                        movement_states[(int)binds.up] = true;
                        break;
                    case binds.down:
                        movement_states[(int)binds.down] = true;
                        break;
                    case binds.bomb:
                        if (player == 1) player1.PlaceBomb();
                        else if (player == 2) player2.PlaceBomb();
                        break;

                }

            }
        }
    }

    /// <summary>
    /// Key release handler
    /// </summary>
    public void OnKeyRelease(Key key)
    {
        foreach (var (player, mapped_key, bind) in player_keys)
        {
            if (key == mapped_key)
            {
                if (bind != binds.bomb) // Bomb is an action, no need to clear
                {
                    var movement_states = player_movement_states[player - 1];
                    movement_states[(int)bind] = false;
                }
            }
        }
    }

    /// <summary>
    /// Creates the grid with breakable and unbreakable walls, as well as all the ground tiles
    /// </summary>
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
                if ((i == 0 || i == rows - 1 || j == 0 || j == cols - 1) || (i % 2 == 0 && j % 2 == 0))
                {
                    grid[i, j] = (int)Tiles.UnbreakableWall;
                }
                else
                {
                    bool spawn_check = ((i <= 1 && j <= 1) || (i <= 1 && j >= cols - 2) || (i >= rows - 2 && j <= 1)) ||
                                       (i >= rows - 2 && j >= cols - 2);

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

    /// <summary>
    /// Used to check if the player or bomb has collided with anything
    /// </summary>
    public bool CollisionCheck(System.Drawing.Rectangle player_rect, Player player)
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (grid[i, j] == (int)Tiles.UnbreakableWall || grid[i, j] == (int)Tiles.BreakableWall)
                {
                    var rect = new System.Drawing.Rectangle(j * GameConfig.TILE_WIDTH, i * GameConfig.TILE_HEIGHT,
                        GameConfig.TILE_WIDTH, GameConfig.TILE_HEIGHT);
                    if (rect.IntersectsWith(player_rect))
                    {
                        return true;
                    }
                }
            }
        }

        foreach (var bomb in new[] { player1.current_bomb, player2.current_bomb })
        {
            if (bomb != null && bomb.exploded && !bomb.explosion_finished)
            {
                foreach (var (ex, ey) in bomb.explosion_tiles)
                {
                    var tileRect = new System.Drawing.Rectangle(
                        ex * GameConfig.TILE_WIDTH,
                        ey * GameConfig.TILE_HEIGHT,
                        GameConfig.TILE_WIDTH,
                        GameConfig.TILE_HEIGHT
                    );

                    if (player_rect.IntersectsWith(tileRect))
                    {
                        player.is_dead = true;
                        return true;
                    }
                }

                if (bomb.ExplosionRect().IntersectsWith(player_rect))
                {
                    player.is_dead = true;
                    return true;
                }
            }
        }
        
        return false;
    }




}



