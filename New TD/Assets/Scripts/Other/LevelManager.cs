using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;

public class LevelManager : MonoBehaviour
{
    public GameObject levelObject;
    
    [Header("Lists")]
    public Transform listMoveResource;
    public Transform listTower;
    public Transform listEnemy;

    [Header("Scripts")]
    public PlayerStats playerStats;
    public SpawnerEnemy spawnerEnemy;
    public BuildingsPanelManager towerManager;
    public RoadsManager roadsManager;
    public BuidManager buidManager;
    public WaveText waveText;
    public Tilemap floor;
    public Tilemap ores;
    public Warehouse home;
    private LevelParametrs currentLevel;
    private string path;

    void Start()
    {
        path = Application.persistentDataPath + "/save1.txt";
    }

    public void Save()
    {
        SavedLevelParametrs save = CreateSave();

        string json = JsonUtility.ToJson(save);

        File.WriteAllText(path, json);
        Debug.Log("save");
    }

    public void LoadSave(SavedLevelParametrs parametrs)
    {
        Debug.Log("load");
        playerStats.inspectortime = parametrs.time;
        PlayerStats.gridSize = parametrs.level.gridSize;
        LoadLevel(parametrs.level, false);
        roadsManager.LoadRoads(parametrs.GetRoadsGrid());
        buidManager.LoadTowers(parametrs.towers);
        roadsManager.LoadMoveResources(parametrs.moveResources);
        spawnerEnemy.LoadEnemys(parametrs.enemys);
    }

    public void Load()
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SavedLevelParametrs parametrs = JsonUtility.FromJson<SavedLevelParametrs>(json);
            LoadSave(parametrs);
        }
    }

    public void LoadLevel(LevelParametrs parametrs)
    {
        LoadLevel(parametrs, true);
    }
    public void LoadLevel(LevelParametrs parametrs, bool resetTime)
    {
        currentLevel = parametrs;

        playerStats.inspectorGridSize = parametrs.gridSize;
        playerStats.inspectortimeToEnd = parametrs.timeToEnd;
        if (resetTime)
        {
            playerStats.inspectortime = 0f;
        }

        towerManager.SetTowersParameters(parametrs.availableTowers);

        var data = parametrs.floorMap.GetTilesBlock(parametrs.floorMap.cellBounds); //maps
        floor.SetTilesBlock(floor.cellBounds, data);
        data = parametrs.oresMap.GetTilesBlock(parametrs.oresMap.cellBounds);
        ores.SetTilesBlock(ores.cellBounds, data);
        
        Export[] exports = new Export[parametrs.startStoreds.Length]; //home
        for (int i = 0; i < parametrs.startStoreds.Length; i++)
        {
            exports[i] = StoredToExport(parametrs.startStoreds[i]);
        }
        home.exports = exports;

        home.startStoreds = copyStoredArray(parametrs.startStoreds);
        spawnerEnemy.enemyes = parametrs.enemyes; //enemy
        spawnerEnemy.chancesFromTime = parametrs.chancesFromTime;

        waveText.waves = parametrs.waves;

        levelObject.SetActive(true);
    }

    public SavedLevelParametrs CreateSave()
    {
        SavedLevelParametrs parametrs = new SavedLevelParametrs();
        parametrs.level = currentLevel;
        parametrs.time = PlayerStats.time;

        SavedMoveResource[] resources = new SavedMoveResource[listMoveResource.childCount]; //ресурсы
        for(int i = 0; i < listMoveResource.childCount; i++)
        {
            resources[i] = new SavedMoveResource(listMoveResource.GetChild(i).gameObject);
        }
        parametrs.moveResources = resources;

        SavedTower[] towers = new SavedTower[listTower.childCount]; //башни
        for(int i = 0; i < listTower.childCount; i++)
        {
            towers[i] = new SavedTower(listTower.GetChild(i).gameObject);
        }
        parametrs.towers = towers;

        SavedEnemy[] enemys = new SavedEnemy[listEnemy.childCount]; //враги
        for(int i = 0; i < listEnemy.childCount; i++)
        {
            enemys[i] = new SavedEnemy(listEnemy.GetChild(i).gameObject);
        }
        parametrs.enemys = enemys;

        parametrs.SetRoadsGrid(roadsManager.roadsGrid);

        return parametrs;
    }

    public Export StoredToExport(Stored stored)
    {
        Export export = new Export();
        export.resource = stored.resource;
        export.count = 1;

        return export;
    }

    public Stored[] copyStoredArray(Stored[] storeds)
    {
        Stored[] orders = new Stored[storeds.Length];
        for (int i = 0; i < storeds.Length; i++)
        {
            Stored newStored = new Stored(storeds[i].resource);
            newStored.maxCount = storeds[i].maxCount;
            newStored.count = storeds[i].count;
            newStored.will = storeds[i].will;
            orders[i] = newStored;
        }

        return orders;
    }
}
