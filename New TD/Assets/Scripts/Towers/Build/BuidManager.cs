using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class BuidManager : MonoBehaviour
{
    [Header("Objects")]
    public TileBase[] purpose = new TileBase[4];
    public BuildingsPanelManager panelManager;
    public RoadsManager roadsManager;
    public Transform listTowers;

    [Header("Tower")]
    public GameObject home;
    public PlanTile planTile;
    public GameObject planObject;
    private TowerParameters towerParameters;
    private bool towerEnabled = false;

    [Header("Tile maps")]
    public Tilemap map_f;
    public Tilemap map_o;
    public Tilemap map_t;
    public Tilemap map_p;
    public Tilemap map_r;

    [Header("Camera")]
    public Camera mainCamera;
    public Collider bildings;

    
    // [Header("Grids")]
    public static GameObject[,] towersGrid;

    void Start()
    {
        if (towersGrid == null)
        {
            towersGrid = new GameObject[PlayerStats.gridSize*2 + 1, PlayerStats.gridSize*2 + 1];
        }
        SetTower(Vector3Int.zero, home);
    }

    public void SetBuldingTower(TowerParameters parameters)
    {
        towerParameters = parameters;
    }

    public void towerDesable()
    {
        towerEnabled = false;
    }

    public void towerEnable()
    {
        towerEnabled = true;
    }

    void Update()
    {
        if (!towerEnabled)
            return;

        if (Input.GetMouseButton(0) || Input.touchCount == 1)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (!bildings.Raycast(ray, out _, 1f))
            {
                if (towerParameters.type == TowerParameters.Type.road)
                    Build(towerParameters, Vector3Int.zero, true, false, true);
                else
                    Purpose();
            }
            else
            {
                map_p.ClearAllTiles();
            }
        }

        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (!bildings.Raycast(ray, out _, 1f))
            {
                Build(towerParameters, Vector3Int.zero, true, false, true);
            }
        }

        if (Input.GetMouseButtonUp(0)) //постройка
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (!bildings.Raycast(ray, out _, 1f))
            {
                Build(towerParameters, Vector3Int.zero, true, false, true);
            }
        }
    }

    public void Purpose() //отображение этих линий
    {
        if (PlayerStats.pause)
            return;
        
        Vector3 clickWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int clickCellPosition = map_p.WorldToCell(clickWorldPosition);

        map_p.ClearAllTiles();
        if (!map_f.GetTile(clickCellPosition))
            return;

        if ((!map_t.GetTile(clickCellPosition) && (towerParameters.type != TowerParameters.Type.delete)) || (map_t.GetTile(clickCellPosition) && (towerParameters.type == TowerParameters.Type.delete))) //белое
        {
            for (int i = -PlayerStats.gridSize; i <= PlayerStats.gridSize; i++)
            {
                map_p.SetTile(new Vector3Int(i, clickCellPosition.y, clickCellPosition.z), purpose[0]);
            }
            for (int i = -PlayerStats.gridSize; i <= PlayerStats.gridSize; i++)
            {
                map_p.SetTile(new Vector3Int(clickCellPosition.x, i, clickCellPosition.z), purpose[0]);
            }
            map_p.SetTile(clickCellPosition, purpose[1]);
        }
        else //красное
        {
            for (int i = -PlayerStats.gridSize; i <= PlayerStats.gridSize; i++)
            {
                map_p.SetTile(new Vector3Int(i, clickCellPosition.y, clickCellPosition.z), purpose[2]);
            }
            for (int i = -PlayerStats.gridSize; i <= PlayerStats.gridSize; i++)
            {
                map_p.SetTile(new Vector3Int(clickCellPosition.x, i, clickCellPosition.z), purpose[2]);
            }
            map_p.SetTile(clickCellPosition, purpose[3]);
        }
    }

    public GameObject GetTower(Vector3Int CellPosition)
    {
        int x = CellPosition.x + PlayerStats.gridSize;
        int y = CellPosition.y + PlayerStats.gridSize;
        
        if (towersGrid == null)
        {
            towersGrid = new GameObject[PlayerStats.gridSize*2 + 1, PlayerStats.gridSize*2 + 1];
        }
        
        return towersGrid[x, y];
    }

    public void SetTower(Vector3Int CellPosition, GameObject tower)
    {
        int x = CellPosition.x + PlayerStats.gridSize;
        int y = CellPosition.y + PlayerStats.gridSize;
        
        if (towersGrid == null)
        {
            towersGrid = new GameObject[PlayerStats.gridSize*2 + 1, PlayerStats.gridSize*2 + 1];
        }
        
        towersGrid[x, y] = tower;
    }

    public void Build(TowerParameters towerParameters, Vector3Int clickCellPosition, bool plan, bool delete, bool showToBPM)
    {
        Build(towerParameters, clickCellPosition, plan, delete, showToBPM, null);
    }
    public void Build(TowerParameters towerParameters, Vector3Int clickCellPosition, bool plan, bool delete, bool showToBPM, Stored[] startStoreds)
    {
        if (PlayerStats.pause)
            return;
        
        if (delete)
        {
            GetTower(clickCellPosition).GetComponent<Tower>().DeleteTower();
        }

        Tilemap target_map;
        GameObject InstantiatenObject;
        TileBase InstantiatenTile;
        if (towerParameters.type == TowerParameters.Type.road)
        {
            target_map = map_r;
            InstantiatenObject = towerParameters.towerObject;
            InstantiatenTile = towerParameters.tile;

            if (clickCellPosition == Vector3Int.zero)
            {
                Vector3 clickWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                clickCellPosition = target_map.WorldToCell(clickWorldPosition);
            }

            if (!map_f.GetTile(clickCellPosition) || !map_f.GetTile(clickCellPosition + Vector3Int.right + Vector3Int.up))
                return;
        }
        else
        {
            target_map = map_t;
            if (plan)
            {
                InstantiatenObject = planObject;
                InstantiatenTile = planTile;
            }
            else
            {
                InstantiatenObject = towerParameters.towerObject;
                InstantiatenTile = towerParameters.tile;
            }

            if (clickCellPosition == Vector3Int.zero)
            {
                Vector3 clickWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                clickCellPosition = target_map.WorldToCell(clickWorldPosition);
            }
            
            if (!map_f.GetTile(clickCellPosition))
                return;
        }

        if (towerParameters.type != TowerParameters.Type.delete)
        {
            if (!target_map.GetTile(clickCellPosition))
            {
                if (!PlayerStats.fixationSelection)
                {
                    if (showToBPM)
                    {
                        panelManager.Build();
                    }
                }
                
                target_map.SetTile(clickCellPosition, InstantiatenTile);
                if (InstantiatenObject)
                {
                    TileBase tile = target_map.GetTile(clickCellPosition);
                    GameObject towerObj = Instantiate(InstantiatenObject, clickCellPosition, transform.rotation, listTowers);
                    towerObj.GetComponent<Tower>().SetAll(target_map, towerParameters);

                    Warehouse warehousesc = towerObj.GetComponent<Warehouse>();
                    if (warehousesc)
                    {
                        warehousesc.roadsManager = roadsManager;
                        if (startStoreds != null)
                        {
                            warehousesc.startStoreds = startStoreds;
                        }
                    }
                    
                    if (towerParameters.type != TowerParameters.Type.road)
                    {
                        SetTower(clickCellPosition, towerObj);
                    }

                    Drill drillsc = towerObj.GetComponent<Drill>();
                    if (drillsc)
                    {
                        drillsc.map_o = map_o;
                    }
                    
                    PlanObject plansc = towerObj.GetComponent<PlanObject>();
                    if (plansc)
                    {
                        plansc.SetTower(towerParameters);
                        plansc.SetBuidManager(this);
                    }
                }
                
                if (towerParameters.type == TowerParameters.Type.road)
                {
                    roadsManager.InstantRoad(clickCellPosition);
                }
            }
        }
        else //снос
        {
            GameObject towerObj = GetTower(clickCellPosition);
            if (towerObj)
            {
                towerObj.GetComponent<Tower>().DeleteTower();
            }
        }
        map_p.ClearAllTiles();
    }

    public bool GetTowerEnabled()
    {
        return towerEnabled;
    }

    public void LoadRoads(bool[,] grid, TowerParameters roadParameters)
    {
        for(int x = 0; x < grid.GetLength(0); x++)
        {
            for(int y = 0; y < grid.GetLength(1); y++)
            {
                if (grid[x, y])
                {
                    Vector3Int position = new Vector3Int(x - PlayerStats.gridSize, y - PlayerStats.gridSize, 0);
                    map_r.SetTile(position, roadParameters.tile);
                }
            }
        }
    }
    
    public void LoadTowers(SavedTower[] towers)
    {
        foreach (SavedTower tower in towers)
        {
            Vector3Int position = new Vector3Int(tower.position.x, tower.position.y, 0);
            Build(tower.parameters, position, tower.plan, false, false, tower.warehouse.GetStoreds());
        }
    }
}
