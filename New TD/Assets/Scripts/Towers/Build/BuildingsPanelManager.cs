using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingsPanelManager : MonoBehaviour
{
    public BuidManager buidManager;
    public TowerParameters[] towersParameters;
    public GameObject towerPanel;
    public GameObject[] selects;
    public int select = -1;

    void Start()
    {
        UpdateBuildings();
    }

    public void UpdateBuildings()
    {
        selects = new GameObject[towersParameters.Length];
        for (int i = 0; i < towersParameters.Length; i++)
        {
            BuildingPanel tower = Instantiate(towerPanel, transform).GetComponent<BuildingPanel>();
            TowerParameters parametrs = towersParameters[i];
            tower.SetParameters(parametrs.price, parametrs.icon, i, parametrs.tile, parametrs.towerObject, this);
            selects[i] = tower.GetSelect();
        }
    }

    public void SetTowersParameters(TowerParameters[] newParameters)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject.Destroy(transform.GetChild(i).gameObject);
        }
        
        towersParameters = newParameters;
        UpdateBuildings();
    }

    public void Build()
    {
        buidManager.towerDesable();
        selects[select].SetActive(false);
        select = -1;
    }
    
    public void OnClick(int index, int price, TileBase tile, GameObject obj)
    {
        if (select == index)
        {
            buidManager.towerDesable();
            selects[select].SetActive(false);
            select = -1;
        }
        else
        {
            if (select != -1)
                selects[select].SetActive(false);
            select = index;
            buidManager.SetBuldingTower(price, tile, obj);
            buidManager.towerEnable();
            selects[select].SetActive(true);
        }
    }
}
