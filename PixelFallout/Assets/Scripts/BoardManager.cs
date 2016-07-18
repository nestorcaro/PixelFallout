using UnityEngine;
using System;
using System.Collections.Generic; //Required for using lists
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {
    // Layout randomly generated levels each time the player starts a level

    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    //--------- Declare the size of the board, and game parameters
    public int columns = 8;
    public int rows = 8;
    public Count wallCount = new Count(5, 9);
    public Count foodCount = new Count(1, 5);
    public GameObject exit;
    public GameObject[] floorTiles;
    public GameObject[] foodTiles;
    public GameObject[] wallTiles;
    public GameObject[] outerWallTiles;
    public GameObject[] enemyTiles;

    private Transform boardHolder;      // Required to Child new game objects as the spawn
    private List<Vector3> gridPositions = new List<Vector3>();    // Keep track of all the tiles of the board and whether its occupied

    void InitializeList()
    {
        gridPositions.Clear();
        for (int x = 1; x < columns - 1; x++)
        {
            for (int y = 1; y < rows - 1; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }

    }

    void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform;

        for (int x = -1; x < columns + 1; x++)
        {
            for (int y = -1; y < rows + 1; y++)
            {
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];  //Select at random from the list of floor tiles
                if (x == -1 || x == columns | y == -1 || y == rows)
                {
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];  //Select outer walls at random in the outer rim of the board
                }
                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject; // Create a game object in the spawned tile
                instance.transform.SetParent(boardHolder);  // Set it as a child of BoardHolder
            }
        }
    }

    Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);    // We remove a selected random pos from the grid so it's not selected again
        return randomPosition;
    }

    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        int objectCount = Random.Range(minimum, maximum + 1);            // How many of the given objects are we gonna spawn in a level
        for (int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }

    }
    //------------------------------

    public void SetupScene(int level)
    {
        BoardSetup();
        InitializeList();
        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);
        int enemyCount = (int)Mathf.Log(level, 2f);     // Enemies increment logarithmically with the level
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);   // Both values are the same, so the number is predefined by level and not by random
        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);


    }
}
