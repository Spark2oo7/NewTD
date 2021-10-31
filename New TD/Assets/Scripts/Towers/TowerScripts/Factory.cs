using UnityEngine;

public class Factory : MonoBehaviour
{
    public Warehouse warehouse;
    public Put[] input;
    public Put[] output;
    

    public virtual void Start()
    {
        InvokeRepeating("Сonversion", 1f, 1f);
    }

    private void Сonversion()
    {
        foreach (Put put in input)
        {
            if (!warehouse.TryReceive(put.resource, put.count))
            {
                return;
            }
        }
        foreach (Put put in output)
        {
            if (!warehouse.TryGive(put.resource, put.count))
            {
                return;
            }
        }

        
        foreach (Put put in input)
        {
            warehouse.Receive(put.resource, put.count);
        }
        foreach (Put put in output)
        {
            warehouse.Give(put.resource, put.count, false);
        }
    }
}


[System.Serializable]
public class Put
{
    public Resource resource;
    public int count;
}