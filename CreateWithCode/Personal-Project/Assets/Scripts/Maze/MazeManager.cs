using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Part of the game managers, this one for the maze.
/// </summary>
public class MazeManager : MonoBehaviour {
    public const int MAZE_RANGE = 100;
    public const int TILE_SIZE = 10;

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

    char[,] maze1 = {{'#', '#', '#', '#', '#', '#', '#', '#', '#', ' ', '#', '#', '#', '#', '#', '#', '#', '#', '#', 'E', '#'},
                     {'#', '#', '#', '#', '#', '#', '#', '#', '#', ' ', '#', '#', '#', '#', '#', '#', '#', '#', '#', ' ', '#'},
                     {'#', '#', '#', '#', '#', '#', '#', '#', '#', ' ', '#', '#', '#', '#', '#', '#', '#', '#', '#', ' ', '#'},
                     {'#', '#', '#', '#', '#', '#', '#', '#', '#', ' ', '#', '#', '#', '#', '#', '#', '#', '#', '#', ' ', '#'},
                     {'#', '#', '#', '#', '#', '#', '#', '#', '#', ' ', '#', '#', '#', '#', '#', '#', '#', '#', '#', ' ', '#'},
                     {'#', '#', '#', '#', '#', '#', '#', '#', '#', ' ', '#', '#', '#', '#', '#', '#', '#', '#', '#', ' ', '#'},
                     {'#', '#', '#', '#', '#', '#', '#', '#', '#', ' ', '#', '#', '#', '#', '#', '#', '#', '#', '#', ' ', '#'},
                     {'#', '#', '#', '#', '#', '#', '#', '#', '#', ' ', '#', '#', '#', '#', '#', '#', '#', '#', '#', ' ', '#'},
                     {'#', '#', '#', '#', '#', '#', '#', '#', '#', ' ', '#', '#', '#', '#', '#', '#', '#', '#', '#', ' ', '#'},
                     {'#', '#', '#', '#', '#', '#', '#', '#', '#', ' ', '#', '#', '#', '#', '#', '#', '#', '#', '#', ' ', '#'},
                     {'#', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '#'},
                     {'#', ' ', '#', '#', '#', '#', '#', '#', '#', '#', '#', ' ', '#', '#', '#', '#', '#', '#', '#', '#', '#'},
                     {'#', ' ', '#', '#', '#', '#', '#', '#', '#', '#', '#', ' ', '#', '#', '#', '#', '#', '#', '#', '#', '#'},
                     {'#', ' ', '#', '#', '#', '#', '#', '#', '#', '#', '#', ' ', '#', '#', '#', '#', '#', '#', '#', '#', '#'},
                     {'#', ' ', '#', '#', '#', '#', '#', '#', '#', '#', '#', ' ', '#', '#', '#', '#', '#', '#', '#', '#', '#'},
                     {'#', ' ', '#', '#', '#', '#', '#', '#', '#', '#', '#', ' ', '#', '#', '#', '#', '#', '#', '#', '#', '#'},
                     {'#', ' ', '#', '#', '#', '#', '#', '#', '#', '#', '#', ' ', '#', '#', '#', '#', '#', '#', '#', '#', '#'},
                     {'#', ' ', '#', '#', '#', '#', '#', '#', '#', '#', '#', ' ', '#', '#', '#', '#', '#', '#', '#', '#', '#'},
                     {'#', ' ', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#'},
                     {'#', ' ', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#'},
                     {'#', 'S', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#'}};
    char[,] maze2 = {{'#', '#', '#', '#', ' ', '#', '#', '#', '#', ' ', '#', '#', '#', ' ', '#', '#', '#', '#', '#', ' ', '#'},
                     {'#', '#', '#', '#', ' ', '#', ' ', '#', '#', ' ', '#', '#', '#', ' ', '#', '#', '#', '#', '#', ' ', '#'},
                     {'#', '#', '#', '#', ' ', '#', ' ', '#', '#', ' ', '#', '#', '#', ' ', '#', '#', '#', '#', '#', ' ', '#'},
                     {'S', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '#', ' ', '#', '#', '#', ' ', '#', '#', '#', '#', '#', ' ', '#'},
                     {'#', '#', '#', ' ', '#', ' ', '#', ' ', '#', ' ', '#', '#', '#', ' ', ' ', '#', '#', '#', '#', ' ', '#'},
                     {'#', '#', '#', ' ', '#', ' ', '#', '#', '#', ' ', '#', '#', '#', ' ', ' ', ' ', ' ', ' ', '#', ' ', '#'},
                     {'#', '#', '#', ' ', '#', ' ', ' ', ' ', '#', ' ', '#', '#', '#', '#', ' ', '#', '#', '#', '#', ' ', '#'},
                     {'#', '#', '#', ' ', '#', '#', '#', '#', '#', ' ', '#', '#', '#', '#', ' ', '#', '#', ' ', '#', ' ', '#'},
                     {'#', '#', '#', ' ', '#', '#', '#', '#', '#', ' ', '#', '#', '#', '#', ' ', '#', '#', '#', '#', ' ', '#'},
                     {'#', '#', '#', ' ', '#', '#', '#', '#', '#', ' ', '#', '#', '#', '#', ' ', '#', '#', ' ', '#', ' ', '#'},
                     {'#', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '#'},
                     {'#', ' ', '#', '#', '#', '#', ' ', '#', '#', '#', '#', ' ', '#', '#', ' ', '#', '#', '#', '#', '#', '#'},
                     {'#', ' ', '#', '#', ' ', '#', ' ', '#', '#', '#', '#', ' ', '#', '#', ' ', '#', ' ', ' ', ' ', ' ', ' '},
                     {'#', ' ', '#', '#', ' ', '#', ' ', '#', '#', '#', '#', ' ', '#', '#', ' ', '#', ' ', '#', '#', '#', ' '},
                     {'#', ' ', '#', '#', ' ', ' ', ' ', '#', '#', '#', '#', ' ', '#', '#', ' ', '#', ' ', '#', 'E', '#', ' '},
                     {'#', ' ', '#', '#', ' ', '#', '#', '#', '#', '#', '#', ' ', '#', '#', ' ', '#', ' ', '#', ' ', '#', ' '},
                     {'#', ' ', '#', '#', ' ', '#', '#', '#', '#', '#', '#', ' ', '#', '#', ' ', '#', ' ', '#', ' ', '#', ' '},
                     {'#', ' ', '#', '#', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '#', '#', ' ', '#', ' ', '#', ' ', '#', ' '},
                     {'#', ' ', '#', '#', '#', '#', '#', ' ', '#', '#', '#', '#', '#', '#', ' ', '#', ' ', ' ', ' ', '#', ' '},
                     {'#', ' ', '#', '#', '#', '#', '#', ' ', '#', '#', '#', '#', '#', '#', ' ', '#', '#', '#', '#', '#', ' '},
                     {'#', ' ', '#', ' ', ' ', ' ', ' ', ' ', '#', '#', '#', '#', '#', '#', ' ', ' ', ' ', ' ', ' ', ' ', ' '}};
    /// <summary>
    /// Possible maze spawn positions stored here to avoid recomputing.
    /// </summary>
    List<(int X, int Z)> spawnPositions;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start() {
        // Randomly select a maze.
        maze = Random.value > 0.5 ? maze1 : maze2;

        //! Create the maze
        //  Also get spawn positions
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

    /// <summary>
    /// Get a random maze spawn position.
    /// </summary>
    /// <returns>The maze spawn position as a tuple. Will be the center of a maze square.</returns>
    public (int X, int Z) GetRandomSpawnPosition() {
        int index = Random.Range(0, spawnPositions.Count);
        return spawnPositions[index];
    }

    bool ShouldInstantiateWall(int row, int col) {
        // Only instantiate a wall in this spot if there are no walls in any direction.
        bool wallLeft, wallRight, wallAbove, wallBelow,
             wallAboveRight, wallAboveLeft, wallBelowRight, wallBelowLeft;

        // Check each direction, if it's out of bounds, then there's a wall
        try {
            wallLeft = maze[row, col - 1] == '#';
        } catch (System.IndexOutOfRangeException) {
            wallLeft = true;
        }

        try {
            wallRight = maze[row, col + 1] == '#';
        } catch (System.IndexOutOfRangeException) {
            wallRight = true;
        }

        try {
            wallAbove = maze[row - 1, col] == '#';
        } catch (System.IndexOutOfRangeException) {
            wallAbove = true;
        }

        try {
            wallBelow = maze[row + 1, col] == '#';
        } catch (System.IndexOutOfRangeException) {
            wallBelow = true;
        }

        try {
            wallAboveRight = maze[row - 1, col + 1] == '#';
        } catch (System.IndexOutOfRangeException) {
            wallAboveRight = true;
        }

        try {
            wallAboveLeft = maze[row - 1, col - 1] == '#';
        } catch (System.IndexOutOfRangeException) {
            wallAboveLeft = true;
        }

        try {
            wallBelowRight = maze[row + 1, col + 1] == '#';
        } catch (System.IndexOutOfRangeException) {
            wallBelowRight = true;
        }

        try {
            wallBelowLeft = maze[row + 1, col - 1] == '#';
        } catch (System.IndexOutOfRangeException) {
            wallBelowLeft = true;
        }

        return !(wallLeft && wallRight && wallAbove && wallBelow
                 && wallAboveRight && wallAboveLeft && wallBelowRight && wallBelowLeft);
    }
}
