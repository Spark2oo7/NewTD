using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPanelManager : MonoBehaviour
{
    public GameObject selectTowerPanel;
    public BuildingsPanelManager panelManager;
    public TowerParameters[] towersParameters;
    public TowerParameters delete;
    public GameObject selectPanel;
    public GameObject[] selects;
    public int select = -1;

    void Start()
    {
        PlayerStats.Pause();
        selects = new GameObject[towersParameters.Length];
        for (int i = 0; i < towersParameters.Length; i++)
        {
            SelectPanel tower = Instantiate(selectPanel, transform).GetComponent<SelectPanel>();
            TowerParameters parametrs = towersParameters[i];
            tower.SetParameters(parametrs.icon, i, this);
            selects[i] = tower.GetSelect();
        }
    }

    public void UpdateBuildings()
    {
        int count = 0;
        for (int i = 0; i < towersParameters.Length; i++)
        {
            if (selects[i].activeSelf)
            {
                count += 1;
            }
        }

        TowerParameters[] newTowersParameters = new TowerParameters[count+1];

        int namber = 0;
        for (int i = 0; i < towersParameters.Length; i++)
        {
            if (selects[i].activeSelf)
            {
                newTowersParameters[namber] = towersParameters[i];
                namber += 1;
            }
        }
        newTowersParameters[namber] = delete;

        panelManager.SetTowersParameters(newTowersParameters);
    }
    
    public void OnClick(int index)
    {
        selects[index].SetActive(!selects[index].activeSelf);
        UpdateBuildings();
    }

    public void Done()
    {
        PlayerStats.Play();
        selectTowerPanel.SetActive(false);
    }
}
