using UnityEngine;

public class Panel : MonoBehaviour
{
    public int moneyPerSec;

    void Start()
    {
        InvokeRepeating("AddMoney", 1f, 1f);
    }

    void AddMoney()
    {
        PlayerStats.money += moneyPerSec;
    }
}
