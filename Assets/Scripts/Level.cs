using UnityEngine;

public class Level : MonoBehaviour
{
    public int width = 5;
    public int height = 5;
    public GameObject[] tilePrefabs;
    public RoomTile[,] roomGrid;

    void Start()
    {
        GenerateLevel();
    }

    void GenerateLevel()
    {
        roomGrid = new RoomTile[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject tileObj = Instantiate(GetRandomTile(), new Vector3(x * 10, y * 10, 0), Quaternion.identity);
                tileObj.name = $"Room ({x},{y})";
                tileObj.transform.parent = this.transform;

                RoomTile tile = tileObj.GetComponent<RoomTile>();
                if (tile != null)
                {
                    roomGrid[x, y] = tile;
                }
                else
                {
                    Debug.LogWarning("Tile prefab missing RoomTile script!");
                }
            }
        }
    }

    GameObject GetRandomTile()
    {
        return tilePrefabs[Random.Range(0, tilePrefabs.Length)];
    }
}


