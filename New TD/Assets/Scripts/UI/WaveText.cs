using UnityEngine;
using UnityEngine.UI;

public class WaveText : MonoBehaviour
{
    public Text waveText;
    public Wave[] waves = new Wave[0];
    public int nextWaveID = 0;

    void Start()
    {
        waveText.text = "";
    }

    void Update()
    {
        if (waves.Length != nextWaveID)
        {
            waveText.text = "";
            if (PlayerStats.time > waves[nextWaveID].start - 99)
            {
                if (PlayerStats.time < waves[nextWaveID].start)
                {
                    waveText.text = (waves[nextWaveID].start - PlayerStats.time).ToString("0") + "с до начала валны";
                }
                else if(PlayerStats.time < waves[nextWaveID].end)
                {
                    waveText.text = (waves[nextWaveID].end - PlayerStats.time).ToString("0") + "с до конца волны";
                }
                else
                {
                    nextWaveID += 1;
                }
            }
        }
    }
}


[System.Serializable]
public class Wave
{
    public float start;
    public float end;
}

