using UnityEngine;

public class Warehouse : MonoBehaviour
{
    public RoadsManager roadsManager;

    public Tower tower;

    public Order[] orders;
    
    void Start()
    {
        InvokeRepeating("Requirement", 0f, 5f);
    }

    public void Requirement()
    {
        roadsManager.Order(tower.CellPosition);
    }
}

[System.Serializable]
public class Order
{
    public int id;
    public int count;
    public int priority;
}
