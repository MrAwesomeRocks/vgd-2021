using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManager : MonoBehaviour
{
    public const int MAZE_RANGE = 100;
    public const int TILE_SIZE = 10;

    public GameObject StartPlatform { get; protected set; }
    public GameObject FinishPlatform { get; protected set; }

    // Maze stuff
    [SerializeField] GameObject mazeWallPrefab;
    [SerializeField] GameObject startPlatformPrefab;
    [SerializeField] GameObject finishPlatformPrefab;
    [SerializeField] Transform mazeContainer;
    [SerializeField] PlayerController player;
    char[,] maze = {{'#', '#', '#', '#', '#', '#', '#', '#', '#', ' ', '#', '#', '#', '#', '#', '#', '#', '#', '#', 'E', '#'},
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
    List<(int X, int Z)> spawnPositions;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        //! Create the maze
        //  Also get spawn positions
        spawnPositions = new List<(int X, int Z)>(20);

        for (int r = 0; r < maze.GetLength(0); r++)
        {
            for (int c = 0; c < maze.GetLength(1); c++)
            {
                var (posX, posZ) = (MatrixColToUnityX(c), MatrixRowToUnityZ(r));

                if (maze[r, c] == '#')
                {
                    if (ShouldInstantiateWall(r, c))
                    {
                        Instantiate(
                            mazeWallPrefab,
                            new Vector3(posX, mazeWallPrefab.transform.position.y, posZ),
                            mazeWallPrefab.transform.rotation,
                            mazeContainer
                        );
                    }
                }
                else if (maze[r, c] == 'E')
                {
                    FinishPlatform = Instantiate(
                        finishPlatformPrefab,
                        new Vector3(posX, finishPlatformPrefab.transform.position.y, posZ),
                        finishPlatformPrefab.transform.rotation,
                        mazeContainer
                    );
                }
                else if (maze[r, c] == 'S')
                {
                    player.MoveToStartPosition(posX, posZ);
                    StartPlatform = Instantiate(
                        startPlatformPrefab,
                        new Vector3(posX, startPlatformPrefab.transform.position.y, posZ),
                        startPlatformPrefab.transform.rotation,
                        mazeContainer
                    );
                }
                else if (maze[r, c] == ' ')
                {
                    spawnPositions.Add((posX, posZ));
                }
            }
        }

        foreach (var (X, Z) in spawnPositions)
        {
            Debug.Log($"({X}, {Z})");
        }
    }

    public int MatrixRowToUnityZ(int row)
    {
        return 100 - row * 10;
    }

    public int MatrixColToUnityX(int col)
    {
        return 10 * col - 100;
    }

    public (int X, int Z) GetRandomSpawnPosition()
    {
        int index = Random.Range(0, spawnPositions.Count);
        return spawnPositions[index];
    }

    bool ShouldInstantiateWall(int row, int col)
    {
        bool wallLeft, wallRight, wallAbove, wallBelow,
             wallAboveRight, wallAboveLeft, wallBelowRight, wallBelowLeft;

        try
        {
            wallLeft = maze[row, col - 1] == '#';
        }
        catch (System.IndexOutOfRangeException)
        {
            wallLeft = true;
        }

        try
        {
            wallRight = maze[row, col + 1] == '#';
        }
        catch (System.IndexOutOfRangeException)
        {
            wallRight = true;
        }

        try
        {
            wallAbove = maze[row - 1, col] == '#';
        }
        catch (System.IndexOutOfRangeException)
        {
            wallAbove = true;
        }

        try
        {
            wallBelow = maze[row + 1, col] == '#';
        }
        catch (System.IndexOutOfRangeException)
        {
            wallBelow = true;
        }

        try
        {
            wallAboveRight = maze[row - 1, col + 1] == '#';
        }
        catch (System.IndexOutOfRangeException)
        {
            wallAboveRight = true;
        }

        try
        {
            wallAboveLeft = maze[row - 1, col - 1] == '#';
        }
        catch (System.IndexOutOfRangeException)
        {
            wallAboveLeft = true;
        }

        try
        {
            wallBelowRight = maze[row + 1, col + 1] == '#';
        }
        catch (System.IndexOutOfRangeException)
        {
            wallBelowRight = true;
        }

        try
        {
            wallBelowLeft = maze[row + 1, col - 1] == '#';
        }
        catch (System.IndexOutOfRangeException)
        {
            wallBelowLeft = true;
        }

        return !(wallLeft && wallRight && wallAbove && wallBelow
                 && wallAboveRight && wallAboveLeft && wallBelowRight && wallBelowLeft);
    }
}
