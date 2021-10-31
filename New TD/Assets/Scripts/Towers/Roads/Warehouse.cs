using UnityEngine;
using System.Collections.Generic;

public class Warehouse : MonoBehaviour
{
    public RoadsManager roadsManager;
    public Tower tower;

    public Order[] orders;
    public Export[] exports;
    public Stored[] startStoreds;

    public Dictionary<int, Stored> storeds = new Dictionary<int, Stored>();
    
    void Start()
    {
        InvokeRepeating("Requirement", 1f, 1f);
        foreach (Stored stored in startStoreds)
        {
            storeds.Add(stored.resource.id, stored);
        }
    }

#if UNITY_EDITOR
    void Update()
    {
        startStoreds = new Stored[10];
        foreach (int ind in storeds.Keys)
        {
            startStoreds[ind] = storeds[ind];
        }
    }
#endif

    public void Requirement()
    {
        foreach (Order order in orders)
        {
            Resource resource = order.resource;
            bool result = false;
            if (storeds.ContainsKey(resource.id))
            {
                if (storeds[resource.id].will < order.count)
                {
                    result = roadsManager.Order(tower.CellPosition, resource, this);
                }
            }
            else
            {
                result = roadsManager.Order(tower.CellPosition, resource, this);
            }
            if (result)
            {
                WillGive(resource, 1);
            }
        }
    }

    public bool TryGive(Resource resource, int count)
    {
        if(!storeds.ContainsKey(resource.id))
        {
            storeds.Add(resource.id, new Stored(resource));
        }

        return storeds[resource.id].TryGive(count);
    }

    public bool Give(Resource resource, int count, bool willEnabled)
    {
        if(TryGive(resource, count))
        {  
            return storeds[resource.id].Give(count, willEnabled);
        }

        return false;
    }
    
    public bool TryReceive(Resource resource, int count)
    {
        if(storeds.ContainsKey(resource.id))
        {
            return storeds[resource.id].TryReceive(count);
        }

        return false;
    }

    public bool Receive(Resource resource, int count)
    {
        if(TryReceive(resource, count))
        {
            return storeds[resource.id].Receive(count);
        }

        return false;
    }

    public void WillGive(Resource resource, int willCount)
    {
        if(!storeds.ContainsKey(resource.id))
        {
            storeds.Add(resource.id, new Stored(resource));
        }

        storeds[resource.id].WillGive(willCount);
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