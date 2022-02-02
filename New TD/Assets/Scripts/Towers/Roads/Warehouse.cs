using UnityEngine;
using System.Collections.Generic;

public class Warehouse : MonoBehaviour
{
    public RoadsManager roadsManager;
    public Tower tower;

    public Order[] orders;
    public Export[] exports;
    public Stored[] startStoreds;

    public bool waitForRecording = false;

    public Dictionary<Resource, Stored> storeds = new Dictionary<Resource, Stored>();
    
    void Start()
    {
        foreach (Stored stored in startStoreds)
        {
            storeds.Add(stored.resource, stored);
        }

        if (!waitForRecording)
        {
            InvokeRepeating("Requirement", 1f, 1f);
            roadsManager.InstantTower(tower.cellPosition, this);
        }
    }

    public void Inscribe()
    {
        if (waitForRecording)
        {
            InvokeRepeating("Requirement", 1f, 1f);
            roadsManager.InstantTower(tower.cellPosition, this);
        }
    }

// #if UNITY_EDITOR
//     void Update()
//     {
//         startStoreds = new Stored[10];
//         foreach (Resource ind in storeds.Keys)
//         {
//             startStoreds[ind.id].count = storeds[ind].count;
//         }
//     }
// #endif

    public void Requirement()
    {
        foreach (Order order in orders)
        {
            Resource resource = order.resource;
            bool result = false;
            if (storeds.ContainsKey(resource))
            {
                if (storeds[resource].will < order.count)
                {
                    result = roadsManager.Order(tower.cellPosition, resource, this);
                }
            }
            else
            {
                result = roadsManager.Order(tower.cellPosition, resource, this);
            }
            if (result)
            {
                WillGive(resource, 1);
            }
        }
    }

    public bool TryGive(Resource resource, int count)
    {
        if(!storeds.ContainsKey(resource))
        {
            storeds.Add(resource, new Stored(resource));
        }

        return storeds[resource].TryGive(count);
    }

    public bool Give(Resource resource, int count, bool willEnabled)
    {
        if(TryGive(resource, count))
        {  
            return storeds[resource].Give(count, willEnabled);
        }

        return false;
    }
    
    public bool TryReceive(Resource resource, int count)
    {
        if(storeds.ContainsKey(resource))
        {
            return storeds[resource].TryReceive(count);
        }

        return false;
    }

    public bool Receive(Resource resource, int count)
    {
        if(TryReceive(resource, count))
        {
            return storeds[resource].Receive(count);
        }

        return false;
    }

    public void WillGive(Resource resource, int willCount)
    {
        if(!storeds.ContainsKey(resource))
        {
            storeds.Add(resource, new Stored(resource));
        }

        storeds[resource].WillGive(willCount);
    }
}

[System.Serializable]
public class Export
{
    public Resource resource;
    public int count;
}


[System.Serializable]
public class Order
{
    public Resource resource;
    public int count;
}


[System.Serializable]
public class Stored
{
    public Resource resource;
    public int maxCount;
    public int count;
    public int will;

    public Stored(Resource res) 
    {
        resource = res;
        maxCount = 99;
        count = 0;
    }
    
    public Stored(Resource res, int newCount) 
    {
        resource = res;
        maxCount = 99;
        count = newCount;
    }

    public bool TryGive(int plus)
    {
        if (count + plus <= maxCount)
        {
            return true;
        }
        return false;
    }

    public bool Give(int plus, bool willEnabled)
    {
        if (TryGive(plus))
        {
            count += plus;
            if (!willEnabled)
            {
                will += plus;
            }
            return true;
        }
        return false;
    }

    public bool TryReceive(int minus)
    {
        if (count - minus >= 0)
        {
            return true;
        }
        return false;
    }
    
    public bool Receive(int minus)
    {
        if (TryReceive(minus))
        {
            will -= minus;
            count -= minus;
            return true;
        }
        return false;
    }

    public void WillGive(int willCount)
    {
        will += willCount;
    }
}