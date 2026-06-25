using UnityEngine;

public class CityGenerator : MonoBehaviour
{
    [Header("Grid Settings")]
    public int width = 20;
    public int height = 20;
    public float cellSize = 1f;

    [Header("Prefabs")]
    public GameObject roadPrefab;
    public GameObject buildingPrefab;
    public GameObject treePrefab;
    public GameObject castlePrefab;

    private TileType[,] grid;

    enum TileType
    {
        Empty,
        Road,
        Building,
        Tree,
        Castle
    }

    void Start()
    {
        grid = new TileType[width, height];
        GenerateCity();
        SpawnCity();
    }

    void GenerateCity()
    {
        int castleX = width / 2;
        int castleZ = height / 2;

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                if (x % 4 == 0 || z % 4 == 0)
                {
                    grid[x, z] = TileType.Road;
                }
                else if (Mathf.Abs(x - castleX) <= 1 && Mathf.Abs(z - castleZ) <= 1)
                {
                    grid[x, z] = TileType.Castle;
                }
                else if (IsNearRoad(x, z))
                {
                    grid[x, z] = Random.value > 0.2f ? TileType.Building : TileType.Tree;
                }
                else
                {
                    grid[x, z] = Random.value > 0.7f ? TileType.Building : TileType.Tree;
                }
            }
        }
    }

    bool IsNearRoad(int x, int z)
    {
        if (x > 0 && grid[x - 1, z] == TileType.Road) return true;
        if (x < width - 1 && grid[x + 1, z] == TileType.Road) return true;
        if (z > 0 && grid[x, z - 1] == TileType.Road) return true;
        if (z < height - 1 && grid[x, z + 1] == TileType.Road) return true;
        return false;
    }

    void SpawnCity()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                SpawnTile(x, z, grid[x, z]);
            }
        }
    }

    void SpawnTile(int x, int z, TileType type)
    {
        GameObject prefab = null;
        float yOffset = 0f;

        switch (type)
        {
            case TileType.Road:
                prefab = roadPrefab;
                yOffset = 0f;
                break;
            case TileType.Building:
                prefab = buildingPrefab;
                yOffset = 0.5f;
                break;
            case TileType.Tree:
                prefab = treePrefab;
                yOffset = 0.5f;
                break;
            case TileType.Castle:
                prefab = castlePrefab;
                yOffset = 0.5f;
                break;
        }

        if (prefab != null)
        {
            Vector3 position = new Vector3(x * cellSize, yOffset, z * cellSize);
            Instantiate(prefab, position, Quaternion.identity, transform);
        }
    }
}