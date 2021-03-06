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
    public CameraMove cameraMove;
    public Tilemap floor;
    public Tilemap ores;
    public Warehouse home;
    private LevelParametrs currentLevel;

    public void Save(int number)
    {
        SavedLevelParametrs save = CreateSave();

        string json = JsonUtility.ToJson(save);
        string path = GetPath(number, false);

        File.WriteAllText(path, json);

        Shortly shortly = new Shortly(save);

        path = GetPath(number, true);
        json = JsonUtility.ToJson(shortly);

        File.WriteAllText(path, json);
    }

    public void LoadSave(SavedLevelParametrs parametrs)
    {
        playerStats.inspectortime = parametrs.time;
        PlayerStats.gridSize = parametrs.level.gridSize;
        LoadLevel(parametrs.level, false);
        roadsManager.LoadRoads(parametrs.GetRoadsGrid());
        buidManager.LoadTowers(parametrs.towers);
        roadsManager.LoadMoveResources(parametrs.moveResources);
        spawnerEnemy.LoadEnemys(parametrs.enemys);
    }

    public void Load(int number)
    {
        string path = GetPath(number, false);
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

        PlayerStats.gridSize = parametrs.gridSize;
        
        playerStats.inspectortimeToEnd = parametrs.timeToEnd;
        if (resetTime)
        {
            playerStats.inspectortime = 0f;
        }

        towerManager.SetTowersParameters(parametrs.availableTowers);

        parametrs.floorMap.CompressBounds();
        parametrs.oresMap.CompressBounds();

        var data = parametrs.floorMap.GetTilesBlock(parametrs.floorMap.cellBounds); //maps
        floor.SetTilesBlock(parametrs.floorMap.cellBounds, data);
        data = parametrs.oresMap.GetTilesBlock(parametrs.oresMap.cellBounds);
        ores.SetTilesBlock(parametrs.oresMap.cellBounds, data);
        
        Export[] exports = new Export[parametrs.startStoreds.Length]; //home
        for (int i = 0; i < parametrs.startStoreds.Length; i++)
        {
            exports[i] = StoredToExport(parametrs.startStoreds[i]);
        }
        home.exports = exports;

        home.startStoreds = copyStoredArray(parametrs.startStoreds);
        spawnerEnemy.enemyes = parametrs.enemyes; //enemy
        spawnerEnemy.chancesFromTime = parametrs.chancesFromTime;
        spawnerEnemy.spawnPositionsTilemap = parametrs.spawnPositions;
        cameraMove.SetSize();

        waveText.waves = parametrs.waves;

        levelObject.SetActive(true);
    }

    public SavedLevelParametrs CreateSave()
    {
        SavedLevelParametrs parametrs = new SavedLevelParametrs();
        parametrs.level = currentLevel;
        parametrs.time = PlayerStats.time;

        SavedMoveResource[] resources = new SavedMoveResource[listMoveResource.childCount]; //??????????????
        for(int i = 0; i < listMoveResource.childCount; i++)
        {
            resources[i] = new SavedMoveResource(listMoveResource.GetChild(i).gameObject);
        }
        parametrs.moveResources = resources;

        SavedTower[] towers = new SavedTower[listTower.childCount]; //??????????
        for(int i = 0; i < listTower.childCount; i++)
        {
            towers[i] = new SavedTower(listTower.GetChild(i).gameObject);
        }
        parametrs.towers = towers;

        SavedEnemy[] enemys = new SavedEnemy[listEnemy.childCount]; //??????????
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

    static public string GetPath(int number, bool shortly)
    {
        string path;

        if (shortly)
        {
            path = Application.persistentDataPath + "/shortly" + number.ToString() + ".txt";
        }
        else
        {
            path = Application.persistentDataPath + "/save" + number.ToString() + ".txt";
        }

        return path;
    }
}
