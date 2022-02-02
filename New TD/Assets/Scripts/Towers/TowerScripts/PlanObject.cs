using UnityEngine;

public class PlanObject : MonoBehaviour
{
    public Warehouse warehouse;
    public Tower tower;
    public SpriteRenderer planSprite; 
    public SpriteRenderer towerSprite;
    private TowerParameters towerParameters;
    public BuidManager buidManager;
    public string enemyTag;
    
    public void SetBuidManager(BuidManager bm)
    {
        buidManager = bm;
    }

    public void SetTower(TowerParameters parameters)
    {
        towerParameters = parameters;
        planSprite.sprite = parameters.icon;
        towerSprite.sprite = parameters.icon;

        int len = towerParameters.price.Length;

        Order[] orders = new Order[len];

        for (int i = 0; i < len; i++)
        {
            orders[i] = PutToOrder(towerParameters.price[i]);
        }

        warehouse.orders = orders;
        warehouse.Inscribe();
        InvokeRepeating("Done", 1.5f, 1f);
    }

    public void Done()
    {
        if (Test())
        {
            buidManager.Build(towerParameters, tower.cellPosition, false, true, false);
        }
    }

    public bool Test()
    {
        foreach (Put put in towerParameters.price)
        {
            if (!warehouse.TryReceive(put.resource, put.count))
            {
                return false;
            }
        }
        return true;
    }

    public Order PutToOrder(Put put)
    {
        Order order = new Order();
        order.resource = put.resource;
        order.count = put.count;

        return order;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == enemyTag)
        {
            tower.DeleteTower();
        }
    }
}
