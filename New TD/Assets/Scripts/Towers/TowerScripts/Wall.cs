using UnityEngine;

public class Wall : MonoBehaviour
{
    public Tower tower;
    public float hpRegeneration;
    public float maxHp;

    void Start()
    {
        InvokeRepeating("Regeneration", 0f, 1f);
    }

    private void Regeneration()
    {
        if (tower.hp < maxHp)
        {
            if (maxHp-tower.hp < hpRegeneration)
            {
                tower.hp = maxHp;
            }
            else
            {
                if (PlayerStats.money >= 1)
                {
                    tower.hp += hpRegeneration;
                    PlayerStats.money -= 1;
                }
            }
        }
    }
}
