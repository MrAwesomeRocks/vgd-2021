using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Part of the game managers, this one for the maze.
/// </summary>
public class MazeManager : MonoBehaviour {
    public const int MAZE_RANGE = 100;
    public const int TILE_SIZE = 10;

    // Maze parameters, must be odd
    const int HEIGHT = (MAZE_RANGE / TILE_SIZE) * 2 + 1;
    const int WIDTH = HEIGHT;
    const int NUM_TILES = (MAZE_RANGE / TILE_SIZE) * (MAZE_RANGE / TILE_SIZE);  // Maze width ^ 2

    /// <summary>
    /// The start platform of the maze.
    /// </summary>
    public GameObject StartPlatform { get; protected set; }
    /// <summary>
    /// The finish platform of the maze.
    /// </summary>
    public GameObject FinishPlatform { get; protected set; }

    // Maze prefabs and their container
    [SerializeField] GameObject mazeWallPrefab;
    [SerializeField] GameObject startPlatformPrefab;
    [SerializeField] GameObject finishPlatformPrefab;
    [SerializeField] Transform mazeContainer;

    // The player
    [SerializeField] PlayerController player;

    // The maze and the possible mazes.
    char[,] maze;

    /// <summary>
    /// Possible maze spawn positions stored here to avoid recomputing.
    /// </summary>
    List<(int X, int Z)> spawnPositions;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start() {
        maze = GenerateMaze();
        InstantiateMaze();
    }

    #region Matrix-Unity Converters
    /// <summary>
    /// Go from a maze matrix row to a Unity Z coordinate.
    /// </summary>
    /// <param name="row">The maze matrix row.</param>
    /// <returns>The Unity Z coordinate.</returns>
    public static int MatrixRowToUnityZ(int row) {
        return MAZE_RANGE - row * TILE_SIZE;
    }

    /// <summary>
    /// Go from a maze matrix column to a Unity X coordinate.
    /// </summary>
    /// <param name="col">The maze matrix column.</param>
    /// <returns>The Unity X coordinate.</returns>
    public static int MatrixColToUnityX(int col) {
        return TILE_SIZE * col - MAZE_RANGE;
    }

    /// <summary>
    /// Go from a Unity Z coordinate to a maze matrix row.
    /// </summary>
    /// <param name="z">The Unity Z coordinate.</param>
    /// <returns>The maze matrix row.</returns>
    public static int UnityZToMatrixRow(int z) {
        return (MAZE_RANGE - z) / TILE_SIZE;
    }

    /// <summary>
    /// Go from a Unity X coordinate to a maze matrix column.
    /// </summary>
    /// <param name="x">The Unity X coordinate.</param>
    /// <returns>The maze matrix column.</returns>
    public static int UnityXToMatrixCol(int x) {
        return (x + MAZE_RANGE) / TILE_SIZE;
    }
    #endregion

    // Inspired by https://github.com/john-science/mazelib/blob/main/mazelib/generate/Prims.py
    // and https://en.wikipedia.org/wiki/Maze_generation_algorithm
    #region Maze Generation
    char[,] GenerateMaze() {
        char[,] grid = new char[HEIGHT, WIDTH];
        for (int r = 0; r < HEIGHT; r++) {
            for (int c = 0; c < WIDTH; c++) {
                grid[r, c] = '#';
            }
        }

        // Choose a random starting position
        // As walls are represented in this maze, we need a range from [1, WIDTH - 1], with a step of 2
        // So add 1 to the double range, then divide by 2
        int current_row = (Random.Range(0, 2 * HEIGHT) + 1) / 2;
        int current_col = (Random.Range(0, 2 * WIDTH) + 1) / 2;
        Debug.Log($"Starting at {current_row}, {current_col}");
        grid[current_row, current_col] = ' ';

        // Create a weighted (random) list of adjacent cells
        List<(int Row, int Col)> neighbors = FindNeighbors(current_row, current_col, grid, true);

        // Loop over all cells
        int visited = 1;
        while (visited < NUM_TILES) {
            // Choose a random neighbor
            int neighbor_index = Random.Range(0, neighbors.Count);
            (current_row, current_col) = neighbors[neighbor_index];
            neighbors.RemoveAt(neighbor_index);  // WHY is there no Pop?

            // Mark the neighbor as part of the maze
            grid[current_row, current_col] = ' ';

            // Connect the neighbor a random neighbor in the maze
            var nearest_neighbors = FindNeighbors(current_row, current_col, grid, false);
            if (nearest_neighbors.Count > 0) {
                var (nearest_row, nearest_col) = nearest_neighbors[0];
                grid[(current_row + nearest_row) / 2, (current_col + nearest_col) / 2] = ' ';
            }

            // Update neighbors
            var unvisited = FindNeighbors(current_row, current_col, grid, true);
            neighbors = neighbors.Union(unvisited).ToList();

            // Increment visited
            visited++;
        }

        // Generate the entrances
        int start_col;
        do {
            start_col = (Random.Range(0, 2 * WIDTH) + 1) / 2;
        } while (StartEndTileInWall(grid, 0, start_col));
        grid[0, start_col] = 'S';

        int end_col;
        do {
            end_col = (Random.Range(0, 2 * WIDTH) + 1) / 2;
        } while (StartEndTileInWall(grid, HEIGHT - 1, start_col));
        grid[HEIGHT - 1, end_col] = 'E';

        return grid;
    }

    List<(int Row, int Col)> FindNeighbors(int row, int col, char[,] grid, bool looking_for_wall = false) {
        List<(int Row, int Col)> neighbors = new List<(int Row, int Col)>();
        char is_wall = looking_for_wall ? '#' : ' ';

        if (row > 1 && grid[row - 2, col] == is_wall) {
            neighbors.Add((row - 2, col));
        }
        if (row < HEIGHT - 2 && grid[row + 2, col] == is_wall) {
            neighbors.Add((row + 2, col));
        }
        if (col > 1 && grid[row, col - 2] == is_wall) {
            neighbors.Add((row, col - 2));
        }
        if (col < WIDTH - 2 && grid[row, col + 2] == is_wall) {
            neighbors.Add((row, col + 2));
        }

        // Shuffle the list
        int n = neighbors.Count;
        while (n > 1) {
            n--;
            int k = Random.Range(0, n + 1);
            var temp = neighbors[k];
            neighbors[k] = neighbors[n];
            neighbors[n] = temp;
        }

        Debug.Log($"Found {neighbors.Count} neighbors");
        return neighbors;
    }

    bool StartEndTileInWall(char[,] grid, int row, int col) {
        // Check each direction
        bool wallLeft = SafeWallCheck(grid, row, col - 1);
        bool wallRight = SafeWallCheck(grid, row, col + 1);
        bool wallAbove = SafeWallCheck(grid, row - 1, col);
        bool wallBelow = SafeWallCheck(grid, row + 1, col);
        bool wallAboveLeft = SafeWallCheck(grid, row - 1, col - 1);
        bool wallAboveRight = SafeWallCheck(grid, row - 1, col + 1);
        bool wallBelowLeft = SafeWallCheck(grid, row + 1, col - 1);
        bool wallBelowRight = SafeWallCheck(grid, row + 1, col + 1);

        // Check if in a wall
        bool inWall = wallLeft && wallRight && wallAbove && wallBelow;

        // Check if in a walled-off area
        // Means that there's no walls in the four cardinal directions
        // And no walls diagonally
        bool inWalledOffArea = !((wallAboveLeft && wallAboveRight) || (wallBelowLeft && wallBelowRight))
                                && !wallLeft && !wallRight && !(wallAbove || wallBelow);

        return inWall || inWalledOffArea;
    }
    #endregion

    #region Maze Instantiation
    void InstantiateMaze() {
        // Also get spawn positions
        spawnPositions = new List<(int X, int Z)>(20);

        // Loop through every index of the maze
        for (int r = 0; r < maze.GetLength(0); r++) {
            for (int c = 0; c < maze.GetLength(1); c++) {
                // Get the unity positions of that row and column.
                var (posX, posZ) = (MatrixColToUnityX(c), MatrixRowToUnityZ(r));
                char elem = maze[r, c];

                // Chose action for each type of maze square.
                switch (elem) {
                    // Wall
                    case '#':
                        // Check if it's needed (to have less game objects)
                        if (ShouldInstantiateWall(r, c)) {
                            // Needed, instantiate
                            Instantiate(
                                mazeWallPrefab,
                                new Vector3(posX, mazeWallPrefab.transform.position.y, posZ),
                                mazeWallPrefab.transform.rotation,
                                mazeContainer
                            );
                        }
                        break;

                    // Start platform
                    case 'S':
                        // Move player and instantiate platform
                        player.MoveToStartPosition(posX, posZ);
                        StartPlatform = Instantiate(
                            startPlatformPrefab,
                            new Vector3(posX, startPlatformPrefab.transform.position.y, posZ),
                            startPlatformPrefab.transform.rotation,
                            mazeContainer
                        );
                        break;

                    // End platform
                    case 'E':
                        // instantiate
                        FinishPlatform = Instantiate(
                            finishPlatformPrefab,
                            new Vector3(posX, finishPlatformPrefab.transform.position.y, posZ),
                            finishPlatformPrefab.transform.rotation,
                            mazeContainer
                        );
                        break;

                    // Empty square
                    case ' ':
                        // Possible spawn position
                        spawnPositions.Add((posX, posZ));
                        break;

                    // This should never happen
                    default:
                        throw new System.InvalidOperationException($"Invalid maze element {elem} at position ({r}, {c}).");
                }
            }
        }

        foreach (var (X, Z) in spawnPositions) {
            // Print spawn positions
            Debug.Log($"({X}, {Z})");
        }
    }

    bool ShouldInstantiateWall(int row, int col) {
        // Only instantiate a wall in this spot if there are no walls in any direction.
        bool wallLeft, wallRight, wallAbove, wallBelow,
             wallAboveRight, wallAboveLeft, wallBelowRight, wallBelowLeft;

        // Check each direction
        wallLeft = SafeWallCheck(maze, row, col - 1);
        wallRight = SafeWallCheck(maze, row, col + 1);
        wallAbove = SafeWallCheck(maze, row - 1, col);
        wallBelow = SafeWallCheck(maze, row + 1, col);
        wallAboveLeft = SafeWallCheck(maze, row - 1, col - 1);
        wallAboveRight = SafeWallCheck(maze, row - 1, col + 1);
        wallBelowLeft = SafeWallCheck(maze, row + 1, col - 1);
        wallBelowRight = SafeWallCheck(maze, row + 1, col + 1);

        return !(wallLeft && wallRight && wallAbove && wallBelow
                 && wallAboveRight && wallAboveLeft && wallBelowRight && wallBelowLeft);
    }
    #endregion

    bool SafeWallCheck(char[,] grid, int row, int col) {
        // Check if the square is out of bounds (in the border walls)
        if (row < 0 || row >= grid.GetLength(0) || col < 0 || col >= grid.GetLength(1)) {
            return true;
        }

        // Check if the square is a wall
        return grid[row, col] == '#';
    }

    /// <summary>
    /// Get a random maze spawn position.
    /// </summary>
    /// <returns>The maze spawn position as a tuple. Will be the center of a maze square.</returns>
    public (int X, int Z) GetRandomSpawnPosition() {
        int index = Random.Range(0, spawnPositions.Count);
        return spawnPositions[index];
    }
}
