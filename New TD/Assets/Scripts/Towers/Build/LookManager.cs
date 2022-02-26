using UnityEngine;
using System.Collections.Generic;

public class LookManager : MonoBehaviour
{
    private Warehouse targetWarehouse;
    private Dictionary<Resource, int> nowShow = new Dictionary<Resource, int>();
    public GameObject resourcePanel;
    public Transform grid;

    public void SetTargetTower(GameObject newTargetTower)
    {
        if (newTargetTower == null)
        {
            targetWarehouse = null;
            nowShow = new Dictionary<Resource, int>();
        }
        else
        {
            targetWarehouse = newTargetTower.GetComponent<Warehouse>();
            nowShow = new Dictionary<Resource, int>();
        }
    }

    void Update()
    {
        if (!gameObject.activeSelf)
            return;
        if (targetWarehouse == null)
        {
            if (nowShow.Keys.Count != 0)
            {
                nowShow.Clear();
                for (int i = 0; i < grid.childCount; i++)
                {
                    Destroy(grid.GetChild(i).gameObject);
                }
            }
            return;
        }

        
        if (!DictionaryEquals(nowShow, targetWarehouse.storeds))
        {
            Debug.Log("update");
            nowShow.Clear();
            for (int i = 0; i < grid.childCount; i++)
            {
                Destroy(grid.GetChild(i).gameObject);
            }

            var storeds = targetWarehouse.storeds;
            foreach (Resource resource in storeds.Keys)
            {
                nowShow.Add(resource, storeds[resource].count);
                ResourcePanel resourcePanelsc = Instantiate(resourcePanel, grid).GetComponent<ResourcePanel>();
                resourcePanelsc.SetResource(resource);

                int need = FindInOrders(resource, targetWarehouse.orders);
                if (need != 0)
                {
                    resourcePanelsc.SetNeed(need);
                }
                
                resourcePanelsc.SetCount(storeds[resource].count);
            }
        }
    }

    public bool DictionaryEquals(Dictionary<Resource, int> dictionary1, Dictionary<Resource, Stored> dictionary2)
    {
        if (dictionary1.Keys.Count != dictionary2.Keys.Count)
        {
            return false;
        }

        foreach (Resource resource in dictionary1.Keys)
        {
            if (dictionary1[resource] != dictionary2[resource].count)
            {
                return false;
            }
        }

        return true;
    }

    public int FindInOrders(Resource resource, Order[] orders)
    {
        foreach (Order order in orders)
        {
            if (order.resource == resource)
            {
                return order.count;
            }
        }
        
        return 0;
    }
}
