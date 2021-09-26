using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class SelectPanel : MonoBehaviour
{
    public Image iconImage;
    public int index = 0;
    public TileBase tile;
    public SelectPanelManager panelManager;
    public GameObject select;

    public void SetParameters(Sprite iconTower, int indexTower, SelectPanelManager SMP)
    {
        iconImage.sprite = iconTower;
        index = indexTower;
        panelManager = SMP;
    }

    public GameObject GetSelect()
    {
        return select;
    }

    public void Click()
    {
        panelManager.OnClick(index);
    }
}
