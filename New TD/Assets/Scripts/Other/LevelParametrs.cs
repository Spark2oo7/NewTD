using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelParametrs : MonoBehaviour
{
    public Tilemap floorMap;
    public Tilemap oresMap;
    public int gridSize;
    public string mode;

    public Stored[] startStoreds;
    public TowerParameters[] availableTowers;

    public float timeToEnd;
    public Wave[] waves = new Wave[0];
    public GameObject[] enemyes = new GameObject[0];
    public AnimationCurve[] chancesFromTime = new AnimationCurve[0];
    public Tilemap spawnPositions;
}

public class SavedLevelParametrs
{
    public LevelParametrs level;
    public float time;
    public SavedMoveResource[] moveResources;
    public SavedTower[] towers;
    public SavedEnemy[] enemys;
    public bool[] roadsGrid;

    public bool[,] GetRoadsGrid()
    {
        int size = (int)Mathf.Round(Mathf.Sqrt(roadsGrid.Length));

        bool[,] grid = new bool[size, size];
        
        for(int y = 0; y < size; y++)
        {
            for(int x = 0; x < size; x++)
            {
                grid[x, y] = roadsGrid[x + y*size];
            }
        }

        return grid;
    }
    
    public void SetRoadsGrid(bool[,] grid)
    {
        int size = (int)Mathf.Round(Mathf.Sqrt(grid.Length));
        roadsGrid = new bool[grid.Length];

        for(int y = 0; y < size; y++)
        {
            for(int x = 0; x < size; x++)
            {
                roadsGrid[x + y*size] = grid[x, y];
            }
        }
    }
}

[System.Serializable]
public class SavedMoveResource
{
    public Resource resource;
    public Vector2Int targetWarehousePosition;
    public Vector2Int currentDot;

    public SavedMoveResource(GameObject moveResource)
    {
        MoveResource script = moveResource.GetComponent<MoveResource>();
        Vector3 position = script.GetCurrentDot();
        currentDot = Vector2Int.RoundToInt(new Vector2(position.x - 0.5f, position.y - 0.5f));
        if (script.targetWarehouse)
        {
            position = script.targetWarehouse.gameObject.transform.position;
            targetWarehousePosition = Vector2Int.RoundToInt(new Vector2(position.x, position.y));
        }
        else
        {
            targetWarehousePosition = currentDot;
        }
        resource = script.resource;
    }
}

[System.Serializable]
public class SavedWarehouse
{
    public int[] counts;
    public Resource[] resources;

    public SavedWarehouse(Warehouse warehouse)
    {
        counts = new int[warehouse.storeds.Count];
        resources = new Resource[warehouse.storeds.Count];

        int i = 0;
        foreach (Resource resource in warehouse.storeds.Keys)
        {
            counts[i] = warehouse.storeds[resource].count;
            resources[i] = resource;
            i++;
        }
    }

    public Stored[] GetStoreds()
    {
        Stored[] storeds = new Stored[counts.Length];
        for (int i = 0; i < counts.Length; i++)
        {
            storeds[i] = new Stored(resources[i], counts[i]);
        }

        return storeds;
    }
}

[System.Serializable]
public class SavedTower
{
    public SavedWarehouse warehouse;
    public TowerParameters parameters;
    public float hp;
    public Vector2Int position;
    public bool plan;

    public SavedTower(GameObject tower)
    {
        Tower script = tower.GetComponent<Tower>();
        position = new Vector2Int(script.cellPosition.x, script.cellPosition.y);
        hp = script.hp;
        parameters = script.parameters;

        Warehouse warehouseScript = tower.GetComponent<Warehouse>();
        if (warehouseScript != null)
        {
            warehouse = new SavedWarehouse(warehouseScript);
        }

        plan = tower.GetComponent<PlanObject>();
    }
}

[System.Serializable]
public class SavedEnemy
{
    public int id;
    public float hp;
    public Vector2Int position;
    public SavedEnemy(GameObject enemy)
    {
        Enemy script = enemy.GetComponent<Enemy>();
        hp = script.hp;
        position = Vector2Int.RoundToInt(enemy.transform.position);
    }
}


[System.Serializable]
public class Shortly
{
    public float time;
    public string mode;

    public Shortly(SavedLevelParametrs parametrs)
    {
        time = parametrs.time;
        mode = parametrs.level.mode;
    }
}