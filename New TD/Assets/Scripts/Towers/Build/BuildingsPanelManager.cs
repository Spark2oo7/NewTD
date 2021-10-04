using UnityEngine;          // функция ⇅  -- передаёт данные вверх/вниз по иеархии
using UnityEngine.Tilemaps; // ⇅ функция  -- получает данные сверх/снизу по иеархии

public class BuildingsPanelManager : MonoBehaviour // отвечает за панель стороительства (которая справа)
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

    public void UpdateBuildings() // создаёт маленькие панельки (для каждой башни) ↓
    {
        selects = new GameObject[towersParameters.Length];
        for (int i = 0; i < towersParameters.Length; i++)
        {
            BuildingPanel tower = Instantiate(towerPanel, transform).GetComponent<BuildingPanel>();
            TowerParameters parametrs = towersParameters[i];
            tower.SetParameters(parametrs.price, parametrs.icon, i, parametrs.tile, parametrs.towerObject, parametrs.type, this); // задаёт параметры ↓
            selects[i] = tower.GetSelect();
        }
    }

    public void SetTowersParameters(TowerParameters[] newParameters) // ↑ обновляет панель строительства
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject.Destroy(transform.GetChild(i).gameObject);
        }
        
        towersParameters = newParameters;
        UpdateBuildings();
    }

    public void Build() // передаёт параметры в buildmanager ↑
    {
        buidManager.towerDesable();
        selects[select].SetActive(false);
        select = -1;
    }
    
    public void OnClick(int index, int price, TileBase tile, GameObject obj, TowerParameters.Type type) // ↑ клик на какую-то башню
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
            buidManager.SetBuldingTower(price, tile, obj, type);
            buidManager.towerEnable();
            selects[select].SetActive(true);
        }
    }
}
