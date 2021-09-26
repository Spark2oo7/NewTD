using UnityEngine;
using UnityEngine.UI;

public class MoneyText : MonoBehaviour
{
    public Text moneyText;

    void Update()
    {
        moneyText.text = PlayerStats.money.ToString();
    }
}
