using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class BuildingPanel : MonoBehaviour
{
    public Text priceText;
    public int price;
    public Image iconImage;
    public int index = 0;
    public TileBase tile;
    public GameObject towerObject;
    public TowerParameters.Type type;
    public GameObject select;
    public BuildingsPanelManager panelManager;

    public void SetParameters(int priceTower, Sprite iconTower, int indexTower, TileBase tileTower, GameObject obj, TowerParameters.Type typeTower, BuildingsPanelManager BMP)
    {
        price = priceTower;
        priceText.text = price.ToString();
        iconImage.sprite = iconTower;
        index = indexTower;
        panelManager = BMP;
        tile = tileTower;
        towerObject = obj;
        type = typeTower;
    }

    public GameObject GetSelect()
    {
        return select;
    }

    public void Click()
    {
        panelManager.OnClick(index, price, tile, towerObject, type);
    }
}	
