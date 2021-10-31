using UnityEngine;

public class Wall : MonoBehaviour
{
    public Tower tower;
    public float hpRegeneration;
    public float maxHp;
    public Warehouse warehouse;
    public Resource resource;

    void Start()
    {
        InvokeRepeating("Regeneration", 0f, 1f);
    }

    private void Regeneration()
    {
        if (tower.hp < maxHp)
        {
            if (maxHp-tower.hp >= hpRegeneration)
            {
                if (warehouse.Receive(resource, 1))
                {
                    tower.hp += hpRegeneration;
                }
            }
        }
    }
}
