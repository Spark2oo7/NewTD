using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class BuidManager : MonoBehaviour
{
    [Header("Objects")]
    public TileBase[] purpose = new TileBase[4];
    public BuildingsPanelManager panelManager;
    public RoadsManager roadsManager;

    [Header("Tower")]
    public int towerPrice;
    public TileBase towerTile;
    public GameObject towerObject;
    public TowerParameters.Type towerType;
    public bool towerEnabled = false;

    [Header("Tile maps")]
    public Tilemap map_f;
    public Tilemap map_t;
    public Tilemap map_p;
    public Tilemap map_r;

    [Header("Camera")]
    public Camera mainCamera;
    public Collider bildings;

    public void SetBuldingTower(int price, TileBase tile, GameObject obj, TowerParameters.Type type)
    {
        towerPrice = price;
        towerTile = tile;
        towerObject = obj;
        towerType = type;
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
                if (towerType == TowerParameters.Type.road)
                    Build();
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
                Build();
            }
        }

        if (Input.GetMouseButtonUp(0)) //постройка
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (!bildings.Raycast(ray, out _, 1f))
            {
                Build();
            }
        }
    }

    void Purpose() //отображение этих линий
    {
        if (PlayerStats.pause)
            return;
        
        Vector3 clickWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int clickCellPosition = map_p.WorldToCell(clickWorldPosition);

        map_p.ClearAllTiles();
        if (!map_f.GetTile(clickCellPosition))
            return;

        if ((!map_t.GetTile(clickCellPosition) && (towerType != TowerParameters.Type.delete)) || (map_t.GetTile(clickCellPosition) && (towerType == TowerParameters.Type.delete))) //белое
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

    void Build()
    {
        if (PlayerStats.pause)
            return;
        
        Vector3 clickWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int clickCellPosition;

        Tilemap target_map;
        if (towerType == TowerParameters.Type.road)
        {
            target_map = map_r;

            clickCellPosition = target_map.WorldToCell(clickWorldPosition);

            if (!map_f.GetTile(clickCellPosition) || !map_f.GetTile(clickCellPosition + Vector3Int.right + Vector3Int.up))
                return;
        }
        else
        {
            target_map = map_t;

            clickCellPosition = target_map.WorldToCell(clickWorldPosition);
            
            if (!map_f.GetTile(clickCellPosition))
                return;
        }
        if (towerType != TowerParameters.Type.delete)
        {
            if (!target_map.GetTile(clickCellPosition))
            {
                if (PlayerStats.money >= towerPrice)
                {
                    PlayerStats.money -= towerPrice;
                    target_map.SetTile(clickCellPosition, towerTile);
                    GameObject towerObj = Instantiate(towerObject, clickCellPosition, transform.rotation);
                    towerObj.GetComponent<Tower>().map = target_map;

                    Warehouse warehousesc = towerObj.GetComponent<Warehouse>();

                    if (warehousesc)
                    {
                        warehousesc.roadsManager = roadsManager;
                    }

                    if (towerType == TowerParameters.Type.road)
                    {
                        roadsManager.InstantRoad(clickCellPosition);
                    }
                }
                if (!PlayerStats.fixationSelection)
                    panelManager.Build();
            }
        }
        else //снос
        {
            if (target_map.GetTile(clickCellPosition))
            {
                target_map.SetTile(clickCellPosition, null);
                if (!PlayerStats.fixationSelection)
                    panelManager.Build();
            }
        }
        map_p.ClearAllTiles();
    }
}
