using UnityEngine;

public class BuildingType : MonoBehaviour
{
    public GameObject ui;
    public GameObject select;
    public string title;
    private BuildingTypesManager buildingTypesManager;
    private int id;
    
    public void SetAll(BuildingTypesManager btm, int newId)
    {
        buildingTypesManager = btm;
        id = newId;
    }

    public void Click()
    {
        buildingTypesManager.OnClick(id);
    }

    public void SetSelect(int setId)
    {
        SetSelect(setId == id);
    }
    public void SetSelect(bool active)
    {
        select.SetActive(active);
    }
}
