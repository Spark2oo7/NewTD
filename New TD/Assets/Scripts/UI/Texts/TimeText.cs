using UnityEngine;
using UnityEngine.UI;

public class TimeText : MonoBehaviour
{
    public Text timeText;

    void Update()
    {
        timeText.text = PlayerStats.time.ToString("0") + "с";
    }
}
