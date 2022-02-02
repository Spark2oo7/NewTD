using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class BuildingPanel : MonoBehaviour
{
    private TowerParameters towerParameters;
    public GameObject select;
    public Image iconImage;
    private BuildingsPanelManager panelManager;

    private int index;

    public void SetParameters(TowerParameters parametrs, int id, BuildingsPanelManager BMP)
    {
        towerParameters = parametrs;
        panelManager = BMP;
        iconImage.sprite = parametrs.icon;
        index = id;
    }

    public GameObject GetSelect()
    {
        return select;
    }

    public void Click()
    {
        panelManager.OnClick(towerParameters, index);
    }
}	
