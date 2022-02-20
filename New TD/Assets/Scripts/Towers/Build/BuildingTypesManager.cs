using UnityEngine;

public class BuildingTypesManager : MonoBehaviour
{
    public BuildingType[] buildingTypes;
    public int select = 0;
    static public BuildingType buildingType;
    //0 - place
    //1 - area
    //2 - look

    void Start()
    {
        for (int i = 0; i < buildingTypes.Length; i++)
        {
            buildingTypes[i].SetAll(this, i);
        }
        UpdateSelects(select);
    }

    public void UpdateSelects(int id)
    {
        buildingType = buildingTypes[id];
        if (!buildingType.ui.activeSelf)
        {
            buildingTypes[select].ui.SetActive(false);
            buildingType.ui.SetActive(true);
        }
        select = id;

        for (int i = 0; i < buildingTypes.Length; i++)
        {
            buildingTypes[i].SetSelect(select);
        }
    }

    public void OnClick(int id)
    {
        UpdateSelects(id);
    }
}
